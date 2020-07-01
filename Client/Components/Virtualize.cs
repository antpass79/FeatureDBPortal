using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace FeatureDBPortal.Client.Components
{
    public class Virtualize<TItem> : ComponentBase
    {
        [Parameter] public string TagName { get; set; } = "div";

        [Parameter] public RenderFragment<TItem> ChildContent { get; set; }

        [Parameter] public ICollection<TItem> Items { get; set; }

        [Parameter] public double ItemHeight { get; set; }

        [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> Attributes { get; set; }

        [Inject] IJSRuntime JS { get; set; }

        ElementReference contentElement;
        int numItemsToSkipBefore;
        int numItemsToShow = 10;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            int sequence = 0;
            // Render actual content
            builder.OpenElement(sequence++, TagName);
            builder.AddMultipleAttributes(sequence++, Attributes);

            var translateY = numItemsToSkipBefore * ItemHeight;
            builder.AddAttribute(sequence++, "style", $"transform: translateY({ translateY }px);");
            builder.AddAttribute(sequence, "data-translateY", translateY);
            builder.AddElementReferenceCapture(sequence++, @ref => { contentElement = @ref; });

            // As an important optimization, *don't* use builder.AddContent(seq, ChildContent, item) because that implicitly
            // wraps a new region around each item, which in turn means that @key does nothing (because keys are scoped to
            // regions). Instead, create a single container region and then invoke the fragments directly.
            builder.OpenRegion(sequence++);
            var items = Items.Skip(numItemsToSkipBefore).Take(numItemsToShow);
            foreach (var item in items)
            {
                ChildContent(item)(builder);
            }
            builder.CloseRegion();

            builder.CloseElement();

            // Also emit a spacer that causes the total vertical height to add up to Items.Count()*numItems
            builder.OpenElement(sequence++, "div");
            var numHiddenItems = Items.Count - numItemsToShow;
            builder.AddAttribute(sequence++, "style", $"width: 1px; height: { numHiddenItems * ItemHeight }px;");
            builder.CloseElement();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var objectRef = DotNetObjectReference.Create(this);
                var initResult = await JS.InvokeAsync<ScrollEventArgs>("VirtualizedComponent._initialize", objectRef, contentElement);
                OnScroll(initResult);
            }
        }

        [JSInvokable]
        public void OnScroll(ScrollEventArgs args)
        {
            Task.Run(() =>
            {
                // TODO: Support horizontal scrolling too
                var relativeTop = args.ContainerRect.Top - args.ContentRect.Top;
                numItemsToSkipBefore = Math.Max(0, (int)(relativeTop / ItemHeight));

                var visibleHeight = args.ContainerRect.Bottom - (args.ContentRect.Top + numItemsToSkipBefore * ItemHeight);
                numItemsToShow = (int)Math.Ceiling(visibleHeight / ItemHeight) + 3;

                StateHasChanged();
            });
        }

        public class ScrollEventArgs
        {
            public DOMRect ContainerRect { get; set; }
            public DOMRect ContentRect { get; set; }
        }

        public class DOMRect
        {
            public double Top { get; set; }
            public double Bottom { get; set; }
            public double Left { get; set; }
            public double Right { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
        }
    }
}