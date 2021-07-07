using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;

using FastReport.Utils;
using FastReport;
using FastReport.Export.Html;
using FastReport.Export.PdfSimple;

using Microsoft.AspNetCore.Hosting;

using System.Xml;

using Newtonsoft.Json.Linq;

namespace webapi4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Portfolio : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        public Portfolio(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string connectionString = String.Empty;

            try
            {
                string ReportPath = Environment.GetEnvironmentVariable("ReportPath");
                connectionString = Program.GetReportSqlConnection(Path.Combine(ReportPath, "appsettings.json"));

                Report report = new Report();
                report.Load(Path.Combine(ReportPath, "Portfolio.frx"));

                using (SqlConnection connection =
                new SqlConnection(connectionString))
                {
                    string queryString = @"
select [ActiveName] = 'Активы на 29.07.2021', [ActiveValue] = 150000.45
union all
select 'Пополнения', 85000.45
union all
select 'Выводы', 85000.45
union all
select 'Дивиденты', 85000.45
union all
select 'Купоны', 2120.11";
                    SqlCommand command = new SqlCommand(queryString, connection);
                    //command.Parameters.AddWithValue("@pricePoint", paramValue);

                    connection.Open();

                    using (SqlDataAdapter sda = new SqlDataAdapter(command))
                    {
                        using (DataSet vDataSet = new DataSet())
                        {
                            // это датасет из БД
                            sda.Fill(vDataSet);

                            vDataSet.Tables[0].TableName = "Employees";


                            report.RegisterData(vDataSet.Tables[0], "Employees");
                            
                        }
                    }

                    string queryString2 = @"
select [ActiveName] = 'Активы2 на 29.07.2021', [ActiveValue] = 150000.45
union all
select 'Пополнения2', 85000.45
union all
select 'Выводы2', 85000.45
union all
select 'Дивиденты2', 85000.45
union all
select 'Купоны2', 2120.11";

                    SqlCommand command2 = new SqlCommand(queryString2, connection);
                    using (SqlDataAdapter sda2 = new SqlDataAdapter(command2))
                    {
                        using (DataSet vDataSet2 = new DataSet())
                        {
                            // это датасет из БД
                            sda2.Fill(vDataSet2);

                            vDataSet2.Tables[0].TableName = "Seconddata";

                            report.RegisterData(vDataSet2.Tables[0], "Seconddata");
                        }
                    }
                }

                report.Prepare();

                using (PDFSimpleExport pdfExport = new PDFSimpleExport())
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        pdfExport.Export(report, stream);
                        stream.Flush();

                        // Тип файла - content-type
                        string file_type = "application/pdf";
                        // Имя файла - необязательно
                        string file_name = "Employees.pdf";

                        return File(stream.ToArray(), file_type, file_name);
                    }
                }
            }
            catch (Exception exception)
            {
                var messages = new List<string>();
                do
                {
                    messages.Add(exception.Message);
                    exception = exception.InnerException;
                }
                while (exception != null);
                var message = string.Join(" - ", messages);

                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);
                //writer.Write(ex.Message + " - " + rows);
                writer.Write(message);
                writer.Flush();
                stream.Position = 0;
                return File(stream, "application/json");
            }
        }
    }
}