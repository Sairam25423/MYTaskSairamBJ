using MYTaskSairamBolloju.App_Start;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MYTaskSairamBolloju.MYTSK
{
    public partial class Registration : System.Web.UI.Page
    {
        DataAccessLayer objBL = new DataAccessLayer();
        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridData();
                txtDOB.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-dd");
                txtDOB.Attributes["min"] = DateTime.Now.ToString("1947-01-01");
            }
        }
        #endregion
        #region BtnCLick Evenet
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            byte[] photo = null;
            if (!(string.IsNullOrEmpty(txtFullName.Text.Trim()) && string.IsNullOrEmpty(txtDOB.Text)) && DpPlace.SelectedIndex > 0 && rblGender.SelectedIndex > 0 && filePhoto.HasFile)
            {
                using (Stream fs = filePhoto.PostedFile.InputStream)
                using (BinaryReader br = new BinaryReader(fs))
                {
                    photo = br.ReadBytes((int)fs.Length);
                }
                string P1 = Convert.ToBase64String(photo);
                DateTime Dt1 = DateTime.Now;
                DateTime Dt2 = Convert.ToDateTime(txtDOB.Text);
                int Age = Dt1.Year - Dt2.Year;
                if (Age > 18)
                {
                    string SP = "Sp_InsertDetails"; string[] ParameterNames = { "@EmpName", "@Gender", "@DOB", "@Place", "@Photo" };
                    string[] ParameterValues = { txtFullName.Text.Trim(), rblGender.SelectedItem.Text, txtDOB.Text, DpPlace.SelectedItem.Text, P1 };
                    bool Result = objBL.InsertData(SP, ParameterNames, ParameterValues);
                    if (Result == true)
                    {
                        Response.Write("<script>alert('Details saved successfully.');</script>"); BindGridData();
                        txtFullName.Text = txtDOB.Text = string.Empty; DpPlace.SelectedIndex = rblGender.SelectedIndex = -1;
                    }
                    else { Response.Write("<script>alert('Something went wrong.')</script>"); }
                }
                else { Response.Write("<script>alert('The age should be greater than 18.')</script>"); }

            }
            else { Response.Write("<script>alert('Please fill all the mandatory fields.')</script>"); }
        }
        #endregion
        #region BindData
        public void BindGridData()
        {
            string SP = "Sp_GetData"; string[] ParameterNames = null; string[] ParameterValues = null;
            DataSet Ds = objBL.RetrievedData(SP, ParameterNames, ParameterValues);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                gvEmployees.DataSource = Ds.Tables[0];
                gvEmployees.DataBind();
                gvEmployees.Visible = true;
            }
        }
        #endregion
        #region RowCancelling
        protected void gvEmployees_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvEmployees.EditIndex = -1;
            BindGridData();
        }
        #endregion
        #region RowDelete
        protected void gvEmployees_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string empId = gvEmployees.DataKeys[e.RowIndex].Value.ToString();
            string SP = "Sp_DeleteDetails"; string[] ParameterNames = { "@EmpId" };
            string[] ParameterValues = { empId };
            bool Result = objBL.InsertData(SP, ParameterNames, ParameterValues);
            if (Result == true)
            {
                gvEmployees.EditIndex = -1;
                BindGridData();
            }
            else { Response.Write("<script>alert('Something went wrong.')</script>"); }
        }
        #endregion
        #region RowUpdate
        protected void gvEmployees_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string P1 = string.Empty;
            GridViewRow row = gvEmployees.Rows[e.RowIndex];
            string empId = (row.FindControl("txtEmpidEdit") as TextBox).Text;
            string empName = (row.FindControl("txtEmpNameEdit") as TextBox).Text.Trim();
            string gender = (row.FindControl("rblGenderEdit") as RadioButtonList).SelectedValue;
            DateTime dob = Convert.ToDateTime((row.FindControl("txtDOBEdit") as TextBox).Text);
            string place = (row.FindControl("ddlPlaceEdit") as DropDownList).SelectedValue;
            byte[] photo = null;
            FileUpload fileUpload = row.FindControl("fileUploadEdit") as FileUpload;
            if (fileUpload != null && fileUpload.HasFile)
            {
                using (Stream fs = fileUpload.PostedFile.InputStream)
                using (BinaryReader br = new BinaryReader(fs))
                {
                    photo = br.ReadBytes((int)fs.Length);
                }
                P1 = Convert.ToBase64String(photo);
            }
            else
            {
                string AB = GetExistingPhoto(empId);
                byte[] byteArray = Encoding.UTF8.GetBytes(AB);
                P1 = Convert.ToBase64String(byteArray);
            }
            string SP = "Sp_UpdateDetails"; string[] ParameterNames = { "@EmpId", "@EmpName", "@Gender", "@DOB", "@Place", "@Photo" };
            string[] ParameterValues = { empId, empName, gender, dob.ToString(), place, P1 };
            bool Result = objBL.InsertData(SP, ParameterNames, ParameterValues);
            if (Result == true)
            {
                gvEmployees.EditIndex = -1;
                BindGridData();
            }
            else { Response.Write("<script>alert('Something went wrong.')</script>"); }
        }
        #endregion
        #region RowEdit
        protected void gvEmployees_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvEmployees.EditIndex = e.NewEditIndex;
            BindGridData();
        }
        #endregion
        #region GettingDetails
        public EMPDetails GetEmployeeDetails(string empId)
        {
            string SP = "Sp_GetIndividualData"; string[] ParameterNames = { "@EmpId" }; string[] ParameterValues = { empId };
            DataSet Ds = objBL.RetrievedData(SP, ParameterNames, ParameterValues);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = Ds.Tables[0].Rows[0];
                EMPDetails employee = new EMPDetails
                {
                    EmpId = row["EmpId"].ToString(),
                    EmpName = row["EmpName"].ToString(),
                    Gender = row["Gender"].ToString(),
                    DOB = row["DOB"].ToString(),
                    Place = row["Place"].ToString(),
                    Photo = row["Photo"] != DBNull.Value ? row["Photo"].ToString() : null
                };
                return employee;
            }
            return null;
        }
        private string GetExistingPhoto(string empId)
        {
            var employeeDetails = GetEmployeeDetails(empId);
            return employeeDetails?.Photo;
        }
        #endregion
        #region RwDataBound
        protected void gvEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                byte[] profilePicture = (byte[])DataBinder.Eval(e.Row.DataItem, "Photo");
                if (profilePicture != null && profilePicture.Length > 0)
                {
                    string base64String = Convert.ToBase64String(profilePicture);
                    Image img = (Image)e.Row.FindControl("imgPhoto");
                    img.ImageUrl = "data:image/png;base64," + base64String;
                }
            }
        }
        #endregion
    }
}