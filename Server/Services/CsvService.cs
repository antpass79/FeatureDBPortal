using CsvHelper;
using CsvHelper.Configuration;
using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureDBPortal.Server.Services
{
    class CsvExportCell
    {
        public AllowModeDTO? AllowMode { get; set; }
        public IEnumerable<CellItemDTO> Items { get; set; }
    }

    class CsvExportColumn
    {
        public string Name { get; set; }
    }

    class CsvExportRow
    {
        public string Name { get; set; }
        public IEnumerable<CsvExportCell> Cells { get; set; }
    }

    public class CsvService : IAsyncCsvService
    {
        private readonly FeaturesContext _context;

        public CsvService(DbContext context)
        {
            _context = context as FeaturesContext;
        }

        async public Task<byte[]> BuildCsv(CsvExportDTO csvExport)
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);
            using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.CurrentCulture)))
            {
                await WriteFiltersAsync(csvWriter, csvExport.Search);
                await WriteHeaderAsync(csvWriter, csvExport.Combination.IntersectionTitle, csvExport.Combination.Columns);
                await WriteRecordsAsync(csvWriter, csvExport.Combination.Rows);

                await csvWriter.FlushAsync();
            }

            byte[] csv = memoryStream.ToArray();

            return await Task.FromResult(csv);
        }

        async Task WriteFiltersAsync(CsvWriter csvWriter, CombinationSearchDTO search)
        {
            IQueryableCombination model = await _context.LogicalModel.FindAsync(search.ModelId);
            IQueryableCombination country = await _context.Country.FindAsync(search.CountryId);
            var user = search.UserLevel;
            IQueryableCombination application = await _context.Application.FindAsync(search.ApplicationId);
            IQueryableCombination probe = await _context.Probe.FindAsync(search.ProbeId);
            IQueryableCombination kit = await _context.BiopsyKits.FindAsync(search.KitId);
            IQueryableCombination option = await _context.Option.FindAsync(search.OptionId);
            IQueryableCombination version = await _context.MinorVersionAssociation.FindAsync(search.VersionId);

            var rowGroup = search.RowLayout;
            var columnGroup = search.ColumnLayout;
            var cellGroup = search.CellLayout;

            WriteField(csvWriter, "Model", model);
            WriteField(csvWriter, "Country", country);
            if (user != UserLevelDTO.None)
                csvWriter.WriteField($"UserLevel: {user}");
            WriteField(csvWriter, "Application", application);
            WriteField(csvWriter, "Probe", probe);
            WriteField(csvWriter, "Kit", kit);
            WriteField(csvWriter, "Option", option);
            WriteField(csvWriter, "Version", version);
            csvWriter.NextRecord();

            if (rowGroup != LayoutTypeDTO.None)
            {
                csvWriter.WriteField($"Rows: {rowGroup}");
                csvWriter.NextRecord();
            }
            if (columnGroup != LayoutTypeDTO.None)
            {
                csvWriter.WriteField($"Columns: {columnGroup}");
                csvWriter.NextRecord();
            }
            if (cellGroup != LayoutTypeDTO.None)
            {
                csvWriter.WriteField($"Cells: {cellGroup}");
                csvWriter.NextRecord();
            }

            csvWriter.NextRecord();

            await Task.CompletedTask;
        }

        async Task WriteHeaderAsync(CsvWriter csvWriter, string intersectionTitle, IEnumerable<ColumnDTO> columns)
        {
            var csvColumns = columns.Select(column => column.Name);

            csvWriter.WriteField(intersectionTitle);

            foreach (var column in csvColumns)
            {
                csvWriter.WriteField(column);
            }

            csvWriter.NextRecord();

            await Task.CompletedTask;
        }

        async Task WriteRecordsAsync(CsvWriter csvWriter, IEnumerable<RowDTO> rows)
        {
            var csvRows = rows
                .Select(row => new
                {
                    Name = row.Title.Name,
                    Cells = row.Cells?.Select(cell => new { AllowMode = cell.AllowMode, Items = cell.Items })
                });

            foreach (var row in csvRows)
            {
                csvWriter.WriteField(row.Name);

                foreach(var cell in row.Cells)
                {
                    if (cell.Items == null)
                        csvWriter.WriteField(cell.AllowMode);
                    else
                    {
                        string concatItems = string.Join(", ", cell.Items.Select(item => item.Name));
                        csvWriter.WriteField(concatItems);
                    }
                }

                csvWriter.NextRecord();
            }

            await Task.CompletedTask;
        }

        private void WriteField(CsvWriter csvWriter, string title, IQueryableCombination field)
        {
            if (field == null)
                return;

            var fullField = $"{title}: {field.Name}";
            csvWriter.WriteField(fullField);
        }
    }
}
