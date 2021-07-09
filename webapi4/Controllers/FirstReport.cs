﻿using Microsoft.AspNetCore.Mvc;
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
    public class FirstReport : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        public FirstReport(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string connectionString = String.Empty;

            try
            {
                //string reportsFolder = String.Empty; // = FindReportsFolder(); важно

                string SettingsStr = System.IO.File.ReadAllText("appsettings.json");


                //System.Xml.XmlDocument xmlDoc = JsonConvert.DeserializeXmlNode(SettingsStr);

                var parsed = JObject.Parse(SettingsStr);

                string DataSource = parsed.SelectToken("SqlConnect.DataSource").Value<string>();
                string InitialCatalog = parsed.SelectToken("SqlConnect.InitialCatalog").Value<string>();
                string UserID = parsed.SelectToken("SqlConnect.UserID").Value<string>();
                string Password = parsed.SelectToken("SqlConnect.Password").Value<string>();

                connectionString += "Data Source=" + DataSource + ";";
                connectionString += "Initial Catalog=" + InitialCatalog + ";";
                connectionString += "User ID=" + UserID + ";";
                connectionString += "Password=" + Password + ";";









                /*
                string queryString =
    @"SELECT
    [AddressID],
    [AddressLine1],
    [PostalCode]
FROM[Person].[Address] AS A
WHERE A.[StateProvinceID] = @pricePoint";

                // Specify the parameter value.
                int paramValue = 7;

                string rows = String.Empty;

                using (SqlConnection connection =
                new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@pricePoint", paramValue);

                    connection.Open();

                    using (SqlDataAdapter sda = new SqlDataAdapter(command))
                    {
                        using (DataSet vDataSet = new DataSet())
                        {
                            // это датасет из БД
                            sda.Fill(vDataSet);
                            DataTable vdt = vDataSet.Tables[0];

                            rows = vdt.Rows.Count.ToString();
                        }
                    }
                }



                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);
                writer.Write("Строк в датасете - " + rows);
                writer.Flush();
                stream.Position = 0;
                return File(stream, "application/json");
                */












                //int test = 1;
                //test = test -1;
                //test = test / test;
                
                Report report = new Report();
                //report.Load(Path.Combine(reportsFolder, "Simple List.frx"));
                string Frxx = "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4KPFJlcG9ydCBTY3JpcHRMYW5ndWFnZT0iQ1NoYXJwIiBUZXh0UXVhbGl0eT0iUmVndWxhciIgUmVwb3J0SW5mby5OYW1lPSJTaW1wbGUgTGlzdCIgUmVwb3J0SW5mby5BdXRob3I9IkZhc3QgUmVwb3J0cyBJbmMiIFJlcG9ydEluZm8uRGVzY3JpcHRpb249IkRlbW9uc3RyYXRlcyBhIHNpbXBsZSBsaXN0IHJlcG9ydC4gVG8gY3JlYXRlIGl0OiYjMTM7JiMxMDstIGdvIHRvICZxdW90O0RhdGEmcXVvdDsgbWVudSBhbmQgc2VsZWN0ICZxdW90O0Nob29zZSBSZXBvcnQgRGF0YS4uLiZxdW90OyBpdGVtIHRvIHNlbGVjdCBhIGRhdGFzb3VyY2U7JiMxMzsmIzEwOy0gZ28gdG8gJnF1b3Q7UmVwb3J0fENvbmZpZ3VyZSBCYW5kcy4uLiZxdW90OyBtZW51IHRvIGNyZWF0ZSB0aGUgYmFuZCBzdHJ1Y3R1cmU7JiMxMzsmIzEwOy0gcmV0dXJuIHRvIHRoZSByZXBvcnQgcGFnZSwgZG91YmxlY2xpY2sgdGhlIGRhdGEgYmFuZCB0byBzaG93IGl0cyBlZGl0b3I7JiMxMzsmIzEwOy0gY2hvb3NlIHRoZSBkYXRhc291cmNlOyYjMTM7JiMxMDstIGRyYWcgZGF0YSBmcm9tIHRoZSBEYXRhIERpY3Rpb25hcnkgd2luZG93IHRvIHRoZSBiYW5kLiIgUmVwb3J0SW5mby5DcmVhdGVkPSIwMS8xNy8yMDA4IDAzOjA1OjU3IiBSZXBvcnRJbmZvLk1vZGlmaWVkPSIwNS8xNC8yMDE5IDEyOjIxOjMzIiBSZXBvcnRJbmZvLkNyZWF0b3JWZXJzaW9uPSIxLjAuMC4wIj4KICA8RGljdGlvbmFyeT4KICAgIDxUYWJsZURhdGFTb3VyY2UgTmFtZT0iRW1wbG95ZWVzIiBSZWZlcmVuY2VOYW1lPSJOb3J0aFdpbmQuRW1wbG95ZWVzIiBEYXRhVHlwZT0iU3lzdGVtLkludDMyIiBFbmFibGVkPSJ0cnVlIj4KICAgICAgPENvbHVtbiBOYW1lPSJFbXBsb3llZUlEIiBEYXRhVHlwZT0iU3lzdGVtLkludDMyIi8+CiAgICAgIDxDb2x1bW4gTmFtZT0iTGFzdE5hbWUiIERhdGFUeXBlPSJTeXN0ZW0uU3RyaW5nIi8+CiAgICAgIDxDb2x1bW4gTmFtZT0iRmlyc3ROYW1lIiBEYXRhVHlwZT0iU3lzdGVtLlN0cmluZyIvPgogICAgICA8Q29sdW1uIE5hbWU9IlRpdGxlIiBEYXRhVHlwZT0iU3lzdGVtLlN0cmluZyIvPgogICAgICA8Q29sdW1uIE5hbWU9IlRpdGxlT2ZDb3VydGVzeSIgRGF0YVR5cGU9IlN5c3RlbS5TdHJpbmciLz4KICAgICAgPENvbHVtbiBOYW1lPSJCaXJ0aERhdGUiIERhdGFUeXBlPSJTeXN0ZW0uRGF0ZVRpbWUiLz4KICAgICAgPENvbHVtbiBOYW1lPSJIaXJlRGF0ZSIgRGF0YVR5cGU9IlN5c3RlbS5EYXRlVGltZSIvPgogICAgICA8Q29sdW1uIE5hbWU9IkFkZHJlc3MiIERhdGFUeXBlPSJTeXN0ZW0uU3RyaW5nIi8+CiAgICAgIDxDb2x1bW4gTmFtZT0iQ2l0eSIgRGF0YVR5cGU9IlN5c3RlbS5TdHJpbmciLz4KICAgICAgPENvbHVtbiBOYW1lPSJSZWdpb24iIERhdGFUeXBlPSJTeXN0ZW0uU3RyaW5nIi8+CiAgICAgIDxDb2x1bW4gTmFtZT0iUG9zdGFsQ29kZSIgRGF0YVR5cGU9IlN5c3RlbS5TdHJpbmciLz4KICAgICAgPENvbHVtbiBOYW1lPSJDb3VudHJ5IiBEYXRhVHlwZT0iU3lzdGVtLlN0cmluZyIvPgogICAgICA8Q29sdW1uIE5hbWU9IkhvbWVQaG9uZSIgRGF0YVR5cGU9IlN5c3RlbS5TdHJpbmciLz4KICAgICAgPENvbHVtbiBOYW1lPSJFeHRlbnNpb24iIERhdGFUeXBlPSJTeXN0ZW0uU3RyaW5nIi8+CiAgICAgIDxDb2x1bW4gTmFtZT0iUGhvdG8iIERhdGFUeXBlPSJTeXN0ZW0uQnl0ZVtdIiBCaW5kYWJsZUNvbnRyb2w9IlBpY3R1cmUiLz4KICAgICAgPENvbHVtbiBOYW1lPSJOb3RlcyIgRGF0YVR5cGU9IlN5c3RlbS5TdHJpbmciLz4KICAgICAgPENvbHVtbiBOYW1lPSJSZXBvcnRzVG8iIERhdGFUeXBlPSJTeXN0ZW0uSW50MzIiLz4KICAgIDwvVGFibGVEYXRhU291cmNlPgogIDwvRGljdGlvbmFyeT4KICA8UmVwb3J0UGFnZSBOYW1lPSJQYWdlMSIgV2F0ZXJtYXJrLkZvbnQ9IkFyaWFsLCA2MHB0Ij4KICAgIDxSZXBvcnRUaXRsZUJhbmQgTmFtZT0iUmVwb3J0VGl0bGUxIiBXaWR0aD0iNzE4LjIiIEhlaWdodD0iMTAzLjk1IiBDYW5Hcm93PSJ0cnVlIj4KICAgICAgPFRleHRPYmplY3QgTmFtZT0iVGV4dDEiIFRvcD0iNzUuNiIgV2lkdGg9IjcxOC4yIiBIZWlnaHQ9IjI4LjM1IiBUZXh0PSJFTVBMT1lFRVMiIEhvcnpBbGlnbj0iQ2VudGVyIiBWZXJ0QWxpZ249IkNlbnRlciIgRm9udD0iVGFob21hLCAxNHB0LCBzdHlsZT1Cb2xkIi8+CiAgICAgIDxUZXh0T2JqZWN0IE5hbWU9IlRleHQxMSIgVG9wPSIyOC4zNSIgV2lkdGg9IjcxOC4yIiBIZWlnaHQ9IjI4LjM1IiBBbmNob3I9IlRvcCwgTGVmdCwgUmlnaHQiIEZpbGwuQ29sb3I9IldoaXRlU21va2UiIENhbkdyb3c9InRydWUiIENhblNocmluaz0idHJ1ZSIgVGV4dD0iW1JlcG9ydC5SZXBvcnRJbmZvLkRlc2NyaXB0aW9uXSYjMTM7JiMxMDsiIFBhZGRpbmc9IjQsIDQsIDQsIDQiIEZvbnQ9IlRhaG9tYSwgOHB0Ii8+CiAgICAgIDxUZXh0T2JqZWN0IE5hbWU9IlRleHQxOCIgV2lkdGg9IjcxOC4yIiBIZWlnaHQ9IjI4LjM1IiBBbmNob3I9IlRvcCwgTGVmdCwgUmlnaHQiIEZpbGwuQ29sb3I9IldoaXRlU21va2UiIEN1cnNvcj0iSGFuZCIgSHlwZXJsaW5rLlZhbHVlPSJodHRwOi8vZmFzdC5yZXBvcnQvYWEyOWUiIEh5cGVybGluay5PcGVuTGlua0luTmV3VGFiPSJ0cnVlIiBUZXh0PSJMZWFybiBob3cgdG8gYnVpbGQgdGhpcyByZXBvcnQgb24gdGhlIEZhc3QgUmVwb3J0cyBBY2FkZW15IGNoYW5uZWwiIFZlcnRBbGlnbj0iQ2VudGVyIiBGb250PSJUYWhvbWEsIDEwcHQsIHN0eWxlPUJvbGQsIFVuZGVybGluZSIgVGV4dEZpbGwuQ29sb3I9IkJsdWUiLz4KICAgICAgPENoaWxkQmFuZCBOYW1lPSJDaGlsZDIiIFRvcD0iMTA3LjY3IiBXaWR0aD0iNzE4LjIiIEhlaWdodD0iMTguOSIvPgogICAgPC9SZXBvcnRUaXRsZUJhbmQ+CiAgICA8RGF0YUJhbmQgTmFtZT0iRGF0YTEiIFRvcD0iMTMwLjI5IiBXaWR0aD0iNzE4LjIiIEhlaWdodD0iMjE5LjI0IiBCb3JkZXIuTGluZXM9IkFsbCIgQm9yZGVyLkNvbG9yPSJNYXJvb24iIENhbkdyb3c9InRydWUiIERhdGFTb3VyY2U9IkVtcGxveWVlcyI+CiAgICAgIDxUZXh0T2JqZWN0IE5hbWU9IlRleHQzIiBMZWZ0PSI5LjQ1IiBUb3A9IjY2LjE1IiBXaWR0aD0iMTAzLjk1IiBIZWlnaHQ9IjE4LjkiIFRleHQ9IkJpcnRoIGRhdGU6IiBWZXJ0QWxpZ249IkNlbnRlciIgRm9udD0iVGFob21hLCA5cHQsIHN0eWxlPUJvbGQiLz4KICAgICAgPFRleHRPYmplY3QgTmFtZT0iVGV4dDQiIExlZnQ9IjExMy40IiBUb3A9IjY2LjE1IiBXaWR0aD0iNDUzLjYiIEhlaWdodD0iMTguOSIgVGV4dD0iW0VtcGxveWVlcy5CaXJ0aERhdGVdIiBGb3JtYXQ9IkRhdGUiIEZvcm1hdC5Gb3JtYXQ9IkQiIFZlcnRBbGlnbj0iQ2VudGVyIiBGb250PSJUYWhvbWEsIDlwdCIvPgogICAgICA8VGV4dE9iamVjdCBOYW1lPSJUZXh0NSIgTGVmdD0iOS40NSIgVG9wPSIxMDMuOTUiIFdpZHRoPSIxMDMuOTUiIEhlaWdodD0iMTguOSIgVGV4dD0iQWRkcmVzczoiIFZlcnRBbGlnbj0iQ2VudGVyIiBGb250PSJUYWhvbWEsIDlwdCwgc3R5bGU9Qm9sZCIvPgogICAgICA8VGV4dE9iamVjdCBOYW1lPSJUZXh0NiIgTGVmdD0iMTEzLjQiIFRvcD0iMTAzLjk1IiBXaWR0aD0iNDUzLjYiIEhlaWdodD0iMTguOSIgQ2FuR3Jvdz0idHJ1ZSIgVGV4dD0iW0VtcGxveWVlcy5BZGRyZXNzXSIgVmVydEFsaWduPSJDZW50ZXIiIEZvbnQ9IlRhaG9tYSwgOXB0Ii8+CiAgICAgIDxUZXh0T2JqZWN0IE5hbWU9IlRleHQ3IiBMZWZ0PSI5LjQ1IiBUb3A9IjEyMi44NSIgV2lkdGg9IjEwMy45NSIgSGVpZ2h0PSIxOC45IiBUZXh0PSJQaG9uZToiIFZlcnRBbGlnbj0iQ2VudGVyIiBGb250PSJUYWhvbWEsIDlwdCwgc3R5bGU9Qm9sZCIvPgogICAgICA8VGV4dE9iamVjdCBOYW1lPSJUZXh0OCIgTGVmdD0iMTEzLjQiIFRvcD0iMTIyLjg1IiBXaWR0aD0iNDUzLjYiIEhlaWdodD0iMTguOSIgVGV4dD0iW0VtcGxveWVlcy5Ib21lUGhvbmVdIiBWZXJ0QWxpZ249IkNlbnRlciIgRm9udD0iVGFob21hLCA5cHQiLz4KICAgICAgPFRleHRPYmplY3QgTmFtZT0iVGV4dDkiIExlZnQ9IjExMy40IiBUb3A9IjE1MS4yIiBXaWR0aD0iNDUzLjYiIEhlaWdodD0iNTYuNyIgQ2FuR3Jvdz0idHJ1ZSIgQ2FuU2hyaW5rPSJ0cnVlIiBUZXh0PSJbRW1wbG95ZWVzLk5vdGVzXSIgUGFkZGluZz0iMiwgMCwgMiwgMTAiIEhvcnpBbGlnbj0iSnVzdGlmeSIgRm9udD0iVGFob21hLCA5cHQiLz4KICAgICAgPFBpY3R1cmVPYmplY3QgTmFtZT0iUGljdHVyZTEiIExlZnQ9IjU3Ni40NSIgVG9wPSI0Ny4yNSIgV2lkdGg9IjEzMi4zIiBIZWlnaHQ9IjE2Mi41NCIgQm9yZGVyLkxpbmVzPSJBbGwiIEJvcmRlci5Db2xvcj0iTWFyb29uIiBDYW5Hcm93PSJ0cnVlIiBDYW5TaHJpbms9InRydWUiIERhdGFDb2x1bW49IkVtcGxveWVlcy5QaG90byIvPgogICAgICA8VGV4dE9iamVjdCBOYW1lPSJUZXh0MTMiIExlZnQ9IjExMy40IiBUb3A9IjQ3LjI1IiBXaWR0aD0iNDUzLjYiIEhlaWdodD0iMTguOSIgVGV4dD0iW0VtcGxveWVlcy5IaXJlRGF0ZV0iIEZvcm1hdD0iRGF0ZSIgRm9ybWF0LkZvcm1hdD0iZCIgVmVydEFsaWduPSJDZW50ZXIiIEZvbnQ9IlRhaG9tYSwgOXB0Ii8+CiAgICAgIDxUZXh0T2JqZWN0IE5hbWU9IlRleHQxNCIgTGVmdD0iMTEzLjQiIFRvcD0iODUuMDUiIFdpZHRoPSI0NTMuNiIgSGVpZ2h0PSIxOC45IiBUZXh0PSJbRW1wbG95ZWVzLkNpdHldIiBWZXJ0QWxpZ249IkNlbnRlciIgRm9udD0iVGFob21hLCA5cHQiLz4KICAgICAgPFRleHRPYmplY3QgTmFtZT0iVGV4dDE1IiBMZWZ0PSI5LjQ1IiBUb3A9IjQ3LjI1IiBXaWR0aD0iMTAzLjk1IiBIZWlnaHQ9IjE4LjkiIFRleHQ9IkhpcmUgZGF0ZToiIFZlcnRBbGlnbj0iQ2VudGVyIiBGb250PSJUYWhvbWEsIDlwdCwgc3R5bGU9Qm9sZCIvPgogICAgICA8VGV4dE9iamVjdCBOYW1lPSJUZXh0MTYiIExlZnQ9IjkuNDUiIFRvcD0iODUuMDUiIFdpZHRoPSIxMDMuOTUiIEhlaWdodD0iMTguOSIgVGV4dD0iQ2l0eToiIFZlcnRBbGlnbj0iQ2VudGVyIiBGb250PSJUYWhvbWEsIDlwdCwgc3R5bGU9Qm9sZCIvPgogICAgICA8VGV4dE9iamVjdCBOYW1lPSJUZXh0MTciIExlZnQ9IjkuNDUiIFRvcD0iMTUxLjIiIFdpZHRoPSIxMDMuOTUiIEhlaWdodD0iMTguOSIgVGV4dD0iTm90ZXM6IiBGb250PSJUYWhvbWEsIDlwdCwgc3R5bGU9Qm9sZCIvPgogICAgICA8VGV4dE9iamVjdCBOYW1lPSJUZXh0MiIgV2lkdGg9IjcxOC4yIiBIZWlnaHQ9IjM3LjgiIEJvcmRlci5MaW5lcz0iQWxsIiBCb3JkZXIuQ29sb3I9Ik1hcm9vbiIgRmlsbD0iTGluZWFyR3JhZGllbnQiIEZpbGwuU3RhcnRDb2xvcj0iSW5kaWFuUmVkIiBGaWxsLkVuZENvbG9yPSJNYXJvb24iIEZpbGwuQW5nbGU9IjkwIiBGaWxsLkZvY3VzPSIwLjUyIiBGaWxsLkNvbnRyYXN0PSIxIiBUZXh0PSJbRW1wbG95ZWVzLkZpcnN0TmFtZV0gW0VtcGxveWVlcy5MYXN0TmFtZV0iIFBhZGRpbmc9IjEwLCAwLCAyLCAwIiBWZXJ0QWxpZ249IkNlbnRlciIgRm9udD0iVGFob21hLCAxMnB0LCBzdHlsZT1Cb2xkIiBUZXh0RmlsbC5Db2xvcj0iV2hpdGUiLz4KICAgICAgPENoaWxkQmFuZCBOYW1lPSJDaGlsZDEiIFRvcD0iMzUzLjI1IiBXaWR0aD0iNzE4LjIiIEhlaWdodD0iMTguOSIvPgogICAgICA8U29ydD4KICAgICAgICA8U29ydCBFeHByZXNzaW9uPSJbRW1wbG95ZWVzLkZpcnN0TmFtZV0iLz4KICAgICAgICA8U29ydCBFeHByZXNzaW9uPSJbRW1wbG95ZWVzLkxhc3ROYW1lXSIvPgogICAgICA8L1NvcnQ+CiAgICA8L0RhdGFCYW5kPgogICAgPFBhZ2VGb290ZXJCYW5kIE5hbWU9IlBhZ2VGb290ZXIxIiBUb3A9IjM3NS44NyIgV2lkdGg9IjcxOC4yIiBIZWlnaHQ9IjI4LjM1IiBGaWxsLkNvbG9yPSJXaGl0ZVNtb2tlIiBDYW5Hcm93PSJ0cnVlIj4KICAgICAgPFRleHRPYmplY3QgTmFtZT0iVGV4dDEwIiBMZWZ0PSI2MTQuMjUiIFdpZHRoPSI5NC41IiBIZWlnaHQ9IjI4LjM1IiBUZXh0PSJbUGFnZU5dIiBIb3J6QWxpZ249IlJpZ2h0IiBWZXJ0QWxpZ249IkNlbnRlciIgRm9udD0iVGFob21hLCA4cHQiLz4KICAgICAgPFRleHRPYmplY3QgTmFtZT0iVGV4dDEyIiBMZWZ0PSI5LjQ1IiBXaWR0aD0iMjE3LjM1IiBIZWlnaHQ9IjI4LjM1IiBDdXJzb3I9IkhhbmQiIEh5cGVybGluay5WYWx1ZT0iaHR0cHM6Ly93d3cuZmFzdC1yZXBvcnQuY29tL2VuL3Byb2R1Y3QvZmFzdC1yZXBvcnQtbmV0LyIgVGV4dD0iR2VuZXJhdGVkIGJ5IEZhc3RSZXBvcnQgLk5FVCIgVmVydEFsaWduPSJDZW50ZXIiIEZvbnQ9IlRhaG9tYSwgOHB0LCBzdHlsZT1VbmRlcmxpbmUiIFRleHRGaWxsLkNvbG9yPSJCbHVlIi8+CiAgICA8L1BhZ2VGb290ZXJCYW5kPgogIDwvUmVwb3J0UGFnZT4KPC9SZXBvcnQ+Cg==";
                byte[] encodedDataAsBytess = System.Convert.FromBase64String(Frxx);
                string Frx = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytess);

                report.LoadFromString(Frx);

                //DataSet data = new DataSet();
                //data.ReadXml(Path.Combine(reportsFolder, "nwind.xml")); // было

                byte[] encodedDataAsBytes = System.Convert.FromBase64String(xmlBa);
                string xml = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);


                //DataSet data = new DataSet();
                //data.ReadXml(XmlReader.Create(new StringReader(xml)));

                //int tc = data.Tables.Count;

                using (SqlConnection connection =
                new SqlConnection(connectionString))
                {
                    string queryString = "select * from FirstReport";
                    SqlCommand command = new SqlCommand(queryString, connection);
                    //command.Parameters.AddWithValue("@pricePoint", paramValue);

                    connection.Open();

                    using (SqlDataAdapter sda = new SqlDataAdapter(command))
                    {
                        using (DataSet vDataSet = new DataSet())
                        {
                            // это датасет из БД
                            sda.Fill(vDataSet);
                            //DataTable vdt = vDataSet.Tables[0];

                            vDataSet.Tables[0].TableName = "Employees";

                            report.RegisterData(vDataSet, "NorthWind");
                            //rows = vdt.Rows.Count.ToString();
                        }
                    }
                }

                //report.RegisterData(data, "NorthWind");

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