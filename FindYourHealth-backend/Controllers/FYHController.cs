using Dapper;
using Dapper.ColumnMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text;


namespace FindYourHealth_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FYHController : ControllerBase
    {
        private readonly ILogger<FYHController> _logger;

        public FYHController(ILogger<FYHController> logger)
        {
            _logger = logger;
            ColumnTypeMapper.RegisterForTypes(typeof(InsuranceAffiliationsModel));
        }

        [HttpGet("SearchResults", Name = "result_table")]
        public JsonResult Result_Table(string slevel, string stype, string srv, string scat, string sscat, string agroup, string insc, string insp, string comp, string _cnty, string sta, string language, int page = 1, int qty = 1)
        {
            IEnumerable<InsuranceAffiliationsModel> Affilitations;
            using (SqlConnection sqlConnection = new SqlConnection(Environment.GetEnvironmentVariable("SQLAZURECONNSTR_fyh_conn")))
            {
                try
                {
                    sqlConnection.Open();
                    DynamicParameters dynamicParameters = new DynamicParameters();
                    StringBuilder builder1 = new StringBuilder(@"SELECT [Service Level] ,[Service Type] ,[Service] ,[Service Category] ,[Service Subcategory] ,[Age Group] ,[Insurance Company] ,[Insurance Plan] ,[Company] ,[Locations] ,[County] ,[Street] ,[St/bldg] ,[City] ,[State] ,[Zip] ,[Phone] ,[Website] ,[Language] FROM [dbo].[vw_Affiliations] WHERE ");
                    if (!string.IsNullOrEmpty(slevel))
                    {
                        builder1.Append("[Service Level] = @slevel AND ");
                        dynamicParameters.Add("slevel", slevel);
                    }
                    if (!string.IsNullOrEmpty(stype))
                    {
                        builder1.Append("[Service Type] = @st AND ");
                        dynamicParameters.Add("st", stype);
                    }
                    if (!string.IsNullOrEmpty(srv))
                    {
                        builder1.Append("[Service] = @srv AND ");
                        dynamicParameters.Add("srv", srv);
                    }
                    if (!string.IsNullOrEmpty(scat))
                    {
                        builder1.Append("[Service Category] = @scat AND ");
                        dynamicParameters.Add("scat", scat);
                    }
                    if (!string.IsNullOrEmpty(sscat))
                    {
                        builder1.Append("[Service Subcategory] = @sscat AND ");
                        dynamicParameters.Add("sscat", sscat);
                    }
                    if (!string.IsNullOrEmpty(agroup))
                    {
                        builder1.Append("[Age Group] = @agroup AND ");
                        dynamicParameters.Add("agroup", agroup);
                    }
                    if (!string.IsNullOrEmpty(insc))
                    {
                        builder1.Append("[Insurance Company] = @insc AND ");
                        dynamicParameters.Add("insc", insc);
                    }
                    if (!string.IsNullOrEmpty(insp))
                    {
                        builder1.Append("[Insurance Plan] = @insp AND ");
                        dynamicParameters.Add("insp", insp);
                    }
                    if (!string.IsNullOrEmpty(comp))
                    {
                        builder1.Append("[Company] = @comp AND ");
                        dynamicParameters.Add("comp", comp);
                    }
                    if (!string.IsNullOrEmpty(_cnty))
                    {
                        builder1.Append("[County] = @cnty AND ");
                        dynamicParameters.Add("cnty", _cnty);
                    }
                    if (!string.IsNullOrEmpty(sta))
                    {
                        builder1.Append("[State] = @sta AND ");
                        dynamicParameters.Add("sta", sta);
                    }
                    if (!string.IsNullOrEmpty(language))
                    {
                        builder1.Append("[Language] = @lang AND ");
                        dynamicParameters.Add("lang", language);
                    }
                    string finalString = builder1.ToString();
                    int lastAND = finalString.LastIndexOf("AND ");
                    finalString = finalString.Remove(lastAND, 4);
                    _logger.LogInformation("Query to be used: " + finalString);
                    Affilitations = sqlConnection.Query<InsuranceAffiliationsModel>(finalString, dynamicParameters, null, true, 0);
                    var pitems = Affilitations
                        .Skip((page - 1) * qty)
                        .Take(qty);
                    var paginatedResults = new PaginatedResults<InsuranceAffiliationsModel>(
                        pitems,
                        page,
                        qty,
                        Affilitations.Count()
                    );
                    return new JsonResult(paginatedResults);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return new JsonResult(ex.Message);
                }
            }
        }

        [HttpGet("Lookup", Name = "Lookup")]
        public JsonResult LookupKV()
        {
            IEnumerable<LookUp> keyValueCollection;
            string query = @"SELECT 'ServiceLevel' as 'Key', [Service Level] as 'Value' FROM [dbo].[Service Level] WHERE [Service Level] IS NOT NULL AND [Service Level] <> '' union SELECT 'ServiceType', [Type] FROM [dbo].[Service Type] WHERE [Type] IS NOT NULL AND [Type] <> '' union SELECT 'Service', [Service] FROM [dbo].[Service] WHERE [Service] IS NOT NULL AND [Service] <> '' union SELECT 'ServiceCategory', [Service Type] FROM [dbo].[Service Category] WHERE [Service Type] IS NOT NULL AND [Service Type] <> '' union SELECT 'ServiceSubcategory', [Service Subcategory] FROM [dbo].[Service Subcategory] WHERE [Service Subcategory] IS NOT NULL AND [Service Subcategory] <> '' union SELECT 'AgeGroup', [Age Group] FROM [dbo].[Age Groups] WHERE [Age Group] IS NOT NULL AND [Age Group] <> '' union SELECT 'InsuranceCompany', [Insurance Company] FROM [dbo].[Insurance Company] WHERE [Insurance Company] IS NOT NULL AND [Insurance Company] <> '' union SELECT 'InsurancePlan', [Plan] FROM [dbo].[Insurance Plans] WHERE [Plan] IS NOT NULL AND [Plan] <> '' union SELECT 'Company', [Company] FROM [dbo].[Company] WHERE [Company] IS NOT NULL AND [Company] <> '' union SELECT 'Location', [Locations] FROM [dbo].[Locations] WHERE [Locations] IS NOT NULL AND [Locations] <> '' union SELECT 'County', [County] FROM [dbo].[County] WHERE [County] IS NOT NULL AND [County] <> '' union SELECT 'Street', [Street] FROM [dbo].[Locations] WHERE [Street] IS NOT NULL AND [Street] <> '' union SELECT 'Suite_Building', [St/bldg] FROM [dbo].[Locations] WHERE [St/bldg] IS NOT NULL AND [St/bldg] <> '' union SELECT 'City', [City] FROM [dbo].[City] WHERE [City] IS NOT NULL AND [City] <> '' union SELECT 'State', [State] FROM [dbo].[State] WHERE [State] IS NOT NULL AND [State] <> '' union SELECT 'Zip', TRIM([Zip]) FROM [dbo].[Zip] WHERE [Zip] IS NOT NULL AND [Zip] <> '' union SELECT 'Phone', [Phone] FROM [dbo].[Locations] WHERE [Phone] IS NOT NULL AND [Phone] <> '' union SELECT 'Language', [Language] FROM [dbo].[Language] WHERE [Language] IS NOT NULL AND [Language] <> ''";
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Environment.GetEnvironmentVariable("SQLAZURECONNSTR_fyh_conn")))
                {
                    keyValueCollection = sqlConnection.Query<LookUp>(query, null, null, true, 0);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new JsonResult(ex.Message);
            }
            return new JsonResult(keyValueCollection);
        }
    }
}
