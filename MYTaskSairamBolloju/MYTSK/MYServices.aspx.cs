using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Web;
using System.Configuration;

namespace MYTaskSairamBolloju.MYTSK
{
    public partial class MYServices : System.Web.UI.Page
    {
        public static string Str = ConfigurationManager.ConnectionStrings["BSR_Tsk"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Form["Id"] != null)
            {
                string Data = string.Empty;
                string Id = Request.Form["Id"].ToString();
                try
                {
                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFile file = Request.Files[0];
                        var FileExtenction = Path.GetExtension(file.FileName).ToString();
                        if (FileExtenction == "jpg" || FileExtenction == "jpeg" || FileExtenction == "png")
                        {
                            decimal Size = Math.Round(((decimal)file.ContentLength / (decimal)1024), 2);
                            if (Size > 1000)
                            {
                                Data = JsonConvert.SerializeObject(new { status = false, Msg = "Image Size Must Be Less Than Or Equal To 1000 KB." });
                            }
                            else
                            {
                                if (Id.Contains("photo"))
                                {
                                    string RootFolder = HttpContext.Current.Server.MapPath("~/Images");
                                    if (!Directory.Exists(RootFolder))
                                        Directory.CreateDirectory(RootFolder);
                                    Id = Id.Replace("photo", "");
                                    if (File.Exists(HttpContext.Current.Server.MapPath(RootFolder + Id + "." + FileExtenction)))
                                    {
                                        File.Delete(HttpContext.Current.Server.MapPath(RootFolder + Id + "." + FileExtenction));
                                    }
                                    else
                                    {
                                        Request.Files[0].SaveAs(Server.MapPath(RootFolder + Id + "." + FileExtenction));
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {

                }
            }
        }
        public static string updateProfilefilePath(string profilepath, string CustomerID)
        {
            string Data = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(Str))
                {
                    using (SqlCommand cmd = new SqlCommand("update_profilepic", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@profilepath ", profilepath);
                        cmd.Parameters.AddWithValue("@Userid", CustomerID);
                        con.Open();
                        int a = cmd.ExecuteNonQuery();
                        con.Close();
                        if (a > 0)
                        {
                            return "Data Saved Successfully.";
                        }
                        else
                        {
                            return "Something Went Wrong.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}