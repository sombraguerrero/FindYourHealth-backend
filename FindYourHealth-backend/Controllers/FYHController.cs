using Dapper;
using Dapper.ColumnMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text;


namespace FindYourHealth_backend.Controllers
{
    [ApiController]
    [Authorize(Policy = "FrontendOnly")]
    [Route("[controller]")]
    public class FYHController : ControllerBase
    {
        private readonly ILogger<FYHController> _logger;

        public FYHController(ILogger<FYHController> logger)
        {
            _logger = logger;
            ColumnTypeMapper.RegisterForTypes(typeof(InsuranceAffiliationsModel));
        }

        [HttpGet("debug-auth")]
        public IActionResult DebugAuth()
        {
            var headers = Request.Headers
                .Where(h => h.Key.StartsWith("X-MS-", StringComparison.OrdinalIgnoreCase))
                .ToDictionary(h => h.Key, h => h.Value.ToString());

            return Ok(headers);
        }


        [HttpGet("SearchResults", Name = "result_table")]
        public JsonResult Result_Table(string ServiceLevel, string ServiceType, string Service, string ServiceCategory, string ServiceSubcategory, string AgeGroup, string InsuranceCompany, string InsurancePlan, string Company, string County, string State, string Language, int page = 1, int qty = 1)
        {
            IEnumerable<InsuranceAffiliationsModel> Affilitations;
            using (SqlConnection sqlConnection = new SqlConnection(Environment.GetEnvironmentVariable("SQLAZURECONNSTR_fyh_conn")))
            {
                try
                {
                    sqlConnection.Open();
                    DynamicParameters dynamicParameters = new DynamicParameters();
                    StringBuilder builder1 = new StringBuilder(@"SELECT [Service Level] ,[Service Type] ,[Service] ,[Service Category] ,[Service Subcategory] ,[Age Group] ,[Insurance Company] ,[Insurance Plan] ,[Company] ,[Locations] ,[County] ,[Street] ,[St/bldg] ,[City] ,[State] ,[Zip] ,[Phone] ,[Website] ,[Language] FROM [dbo].[vw_Affiliations] WHERE ");
                    if (!string.IsNullOrEmpty(ServiceLevel))
                    {
                        builder1.Append("[Service Level] = @slevel AND ");
                        dynamicParameters.Add("slevel", ServiceLevel);
                    }
                    if (!string.IsNullOrEmpty(ServiceType))
                    {
                        builder1.Append("[Service Type] = @st AND ");
                        dynamicParameters.Add("st", ServiceType);
                    }
                    if (!string.IsNullOrEmpty(Service))
                    {
                        builder1.Append("[Service] = @srv AND ");
                        dynamicParameters.Add("srv", Service);
                    }
                    if (!string.IsNullOrEmpty(ServiceCategory))
                    {
                        builder1.Append("[Service Category] = @scat AND ");
                        dynamicParameters.Add("scat", ServiceCategory);
                    }
                    if (!string.IsNullOrEmpty(ServiceSubcategory))
                    {
                        builder1.Append("[Service Subcategory] = @sscat AND ");
                        dynamicParameters.Add("sscat", ServiceSubcategory);
                    }
                    if (!string.IsNullOrEmpty(AgeGroup))
                    {
                        builder1.Append("[Age Group] = @agroup AND ");
                        dynamicParameters.Add("agroup", AgeGroup);
                    }
                    if (!string.IsNullOrEmpty(InsuranceCompany))
                    {
                        builder1.Append("[Insurance Company] = @insc AND ");
                        dynamicParameters.Add("insc", InsuranceCompany);
                    }
                    if (!string.IsNullOrEmpty(InsurancePlan))
                    {
                        builder1.Append("[Insurance Plan] = @insp AND ");
                        dynamicParameters.Add("insp", InsurancePlan);
                    }
                    if (!string.IsNullOrEmpty(Company))
                    {
                        builder1.Append("[Company] = @comp AND ");
                        dynamicParameters.Add("comp", Company);
                    }
                    if (!string.IsNullOrEmpty(County))
                    {
                        builder1.Append("[County] = @cnty AND ");
                        dynamicParameters.Add("cnty", County);
                    }
                    if (!string.IsNullOrEmpty(State))
                    {
                        builder1.Append("[State] = @sta AND ");
                        dynamicParameters.Add("sta", State);
                    }
                    if (!string.IsNullOrEmpty(Language))
                    {
                        builder1.Append("[Language] = @lang AND ");
                        dynamicParameters.Add("lang", Language);
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
            string query = @"SELECT 'ServiceLevel' as 'Key', [Service Level] as 'Value' FROM [dbo].[Service Level] WHERE [Service Level] IS NOT NULL AND [Service Level] <> '' union SELECT 'ServiceType', [Type] FROM [dbo].[Service Type] WHERE [Type] IS NOT NULL AND [Type] <> '' union SELECT 'Service', [Service] FROM [dbo].[Service] WHERE [Service] IS NOT NULL AND [Service] <> '' union SELECT 'ServiceCategory', [Service Type] FROM [dbo].[Service Category] WHERE [Service Type] IS NOT NULL AND [Service Type] <> '' union SELECT 'ServiceSubcategory', [Service Subcategory] FROM [dbo].[Service Subcategory] WHERE [Service Subcategory] IS NOT NULL AND [Service Subcategory] <> '' union SELECT 'AgeGroup', [Age Group] FROM [dbo].[Age Groups] WHERE [Age Group] IS NOT NULL AND [Age Group] <> '' union SELECT 'InsuranceCompany', [Insurance Company] FROM [dbo].[Insurance Company] WHERE [Insurance Company] IS NOT NULL AND [Insurance Company] <> '' union SELECT 'InsurancePlan', [Plan] FROM [dbo].[Insurance Plans] WHERE [Plan] IS NOT NULL AND [Plan] <> '' union SELECT 'Company', [Company] FROM [dbo].[Company] WHERE [Company] IS NOT NULL AND [Company] <> '' union SELECT 'Location', [Locations] FROM [dbo].[Locations] WHERE [Locations] IS NOT NULL AND [Locations] <> '' union SELECT 'County', [County] FROM [dbo].[County] WHERE [County] IS NOT NULL AND [County] <> '' union SELECT 'Street', [Street] FROM [dbo].[Locations] WHERE [Street] IS NOT NULL AND [Street] <> '' union SELECT 'Suite_Building', [St/bldg] FROM [dbo].[Locations] WHERE [St/bldg] IS NOT NULL AND [St/bldg] <> '' union SELECT 'City', [City] FROM [dbo].[City] WHERE [City] IS NOT NULL AND [City] <> '' union SELECT 'State', [State] FROM [dbo].[State] WHERE [State] IS NOT NULL AND [State] <> '' union SELECT 'Zip', TRIM([Zip]) FROM [dbo].[Zip] WHERE [Zip] IS NOT NULL AND [Zip] <> '' union SELECT 'Phone', [Phone] FROM [dbo].[Locations] WHERE [Phone] IS NOT NULL AND [Phone] <> '' union SELECT 'Language', [Language] FROM [dbo].[Language] WHERE [Language] IS NOT NULL AND [Language] <> '' order by [key], [value]";
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
