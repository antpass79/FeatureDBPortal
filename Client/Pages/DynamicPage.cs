using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace FeatureDBPortal.Client.Pages
{
    [RouteAttribute("/dynamicpage")]
     public class DynamicPage : ComponentBase
    {

        public int Counter { get; set; }
        public string InputValue { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "input");
            builder.AddAttribute(1, "type", "text");
            builder.AddAttribute(2, "onkeyup", EventCallback.Factory.Create<KeyboardEventArgs>(this, ChangeCounter ));
            builder.AddAttribute(3, "value", BindConverter.FormatValue(InputValue));
            builder.AddAttribute(4, "oninput", EventCallback.Factory.CreateBinder(this, __value => InputValue = __value, InputValue));
            builder.SetUpdatesAttributeName("value");
            builder.CloseElement();
            builder.AddMarkupContent(5, "\r\n\r\n");
            builder.OpenElement(6, "p");
            builder.AddContent(7, InputValue);
            builder.CloseElement();
            builder.AddMarkupContent(8, "\r\n");
            builder.OpenElement(9, "p");
            builder.AddContent(10, Counter);
            builder.CloseElement();
        }

        private void ChangeCounter(KeyboardEventArgs e)
        {
            Counter++;
        }
    }
}
