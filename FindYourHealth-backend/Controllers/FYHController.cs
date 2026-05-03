using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.Data.SqlClient;
using Dapper;

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
        public JsonResult Result_Table(string slevel, string stype, string srv, string scat, string sscat, string agroup, string insc, string insp, string comp, string _cnty, string sta, string religion)
        {
            IEnumerable<InsuranceAffiliationsModel> Affilitations;
            using (SqlConnection sqlConnection = new SqlConnection(Environment.GetEnvironmentVariable("SQLAZURECONNSTR_fyh_conn")))
            {
                try
                {
                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand() { Connection = sqlConnection };
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
      ,[Religion]
  FROM [dbo].[vw_Affiliations] WHERE ");
                    if (!string.IsNullOrEmpty(slevel))
                    {
                        builder1.Append("[Service Level] = @slevel AND ");
                        cmd.Parameters.Add(new SqlParameter("slevel", slevel));
                    }
                    if (!string.IsNullOrEmpty(stype))
                    {
                        builder1.Append("[Service Type] = @st AND ");
                        cmd.Parameters.Add(new SqlParameter("st", stype));
                    }
                    if (!string.IsNullOrEmpty(srv))
                    {
                        builder1.Append("[Service] = @srv AND ");
                        cmd.Parameters.Add(new SqlParameter("srv", srv));
                    }
                    if (!string.IsNullOrEmpty(scat))
                    {
                        builder1.Append("[Service Category] = @scat AND ");
                        cmd.Parameters.Add(new SqlParameter("scat", scat));
                    }
                    if (!string.IsNullOrEmpty(sscat))
                    {
                        builder1.Append("[Service Subcategory] = @sscat AND ");
                        cmd.Parameters.Add(new SqlParameter("sscat", sscat));
                    }
                    if (!string.IsNullOrEmpty(agroup))
                    {
                        builder1.Append("[Age Group] = @agroup AND ");
                        cmd.Parameters.Add(new SqlParameter("agroup", agroup));
                    }
                    if (!string.IsNullOrEmpty(insc))
                    {
                        builder1.Append("[Insurance Company] = @insc AND ");
                        cmd.Parameters.Add(new SqlParameter("insc", insc));
                    }
                    if (!string.IsNullOrEmpty(insp))
                    {
                        builder1.Append("[Insurance Plan] = @insp AND ");
                        cmd.Parameters.Add(new SqlParameter("insp", insp));
                    }
                    if (!string.IsNullOrEmpty(comp))
                    {
                        builder1.Append("[Company] = @comp AND ");
                        cmd.Parameters.Add(new SqlParameter("comp", comp));
                    }
                    if (!string.IsNullOrEmpty(_cnty))
                    {
                        builder1.Append("[County] = @cnty AND ");
                        cmd.Parameters.Add(new SqlParameter("cnty", _cnty));
                    }
                    if (!string.IsNullOrEmpty(sta))
                    {
                        builder1.Append("[State] = @sta AND ");
                        cmd.Parameters.Add(new SqlParameter("sta", sta));
                    }
                    if (!string.IsNullOrEmpty(religion))
                    {
                        builder1.Append("[Religion] = @religion AND ");
                        cmd.Parameters.Add(new SqlParameter("religion", religion));
                    }
                    string finalString = builder1.ToString();
                    int lastAND = finalString.LastIndexOf("AND ");
                    finalString = finalString.Remove(lastAND, 4);
                    _logger.LogInformation("Query to be used: " + finalString);
                    cmd.CommandText = finalString;

                    SqlDataReader dataReader = cmd.ExecuteReader();
                    Affilitations = sqlConnection.Query<InsuranceAffiliationsModel>(finalString, cmd.Parameters);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return new JsonResult(string.Empty);
                }
            }
            return new JsonResult(Affilitations);
        }
    }
}
