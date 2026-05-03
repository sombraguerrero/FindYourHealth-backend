using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;

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
        }

        [HttpGet("SearchResults", Name = "result_table")]
        public JsonResult Result_Table(string slevel, string stype, string srv, string scat, string sscat, string agroup, string insc, string insp, string comp, string _cnty, string sta, string language)
        {
            IEnumerable<InsuranceAffiliationsModel> Affilitations;
            using (SqlConnection sqlConnection = new SqlConnection(Environment.GetEnvironmentVariable("SQLAZURECONNSTR_fyh_conn")))
            {
                try
                {
                    sqlConnection.Open();
                    DynamicParameters dynamicParameters = new DynamicParameters();
                    StringBuilder builder1 = new StringBuilder(@"SELECT [Service Level]
      ,[Service Type]
      ,[Service]
      ,[Service Category]
      ,[Service Subcategory]
      ,[Age Group]
      ,[Insurance Company]
      ,[Insurance Plan]
      ,[Company]
      ,[Locations]
      ,[County]
      ,[Street]
      ,[St/bldg]
      ,[City]
      ,[State]
      ,[Zip]
      ,[Phone]
      ,[Website]
      ,[Language]
  FROM [dbo].[vw_Affiliations] WHERE ");
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
                    Affilitations = sqlConnection.Query<InsuranceAffiliationsModel>(finalString, dynamicParameters);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return new JsonResult(ex.Message);
                }
            }
            return new JsonResult(Affilitations);
        }
    }
}
