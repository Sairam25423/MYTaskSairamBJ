<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="MYTaskSairamBolloju.MYTSK.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>User Registration</title>
    <!-- Bootstrap 5 CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .form-check input[type="radio"] {
            margin-right: 5px;
        }
    </style>
</head>
<body>
    <form id="form2" runat="server" class="needs-validation">
        <div class="container">
            <div class="card mt-5">
                <div class="card card-head" style="background: #ADBBDA">
                    <h2 class="text-center mt-2 mb-3" style="color: #AC8968">User Registration</h2>
                </div>
                <div class="card card-body" style="background: #EDE8F5">
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 col-md-10 col-lg-4 col-xl-4 col-xxl-4">
                            <label for="txtFullName" class="form-label">Full Name:</label>
                            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="Full name" MaxLength="100" />
                            <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName" InitialValue="" ErrorMessage="Full Name is required" ForeColor="Red" ValidationGroup="SR" />
                            <asp:RegularExpressionValidator ID="revFullName" runat="server" ControlToValidate="txtFullName" ValidationExpression="^[A-Za-z\s]+$" ErrorMessage="Full Name should only contain letters and spaces." ForeColor="Red" ValidationGroup="SR" />
                        </div>
                        <div class="col-xs-12 col-sm-12 col-md-10 col-lg-4 col-xl-4 col-xxl-4">
                            <label for="txtPlace" class="form-label">Place:</label>
                            <asp:DropDownList ID="DpPlace" runat="server" CssClass="form-control">
                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                                <asp:ListItem Text="India" Value="1">India</asp:ListItem>
                                <asp:ListItem Text="Nepal" Value="2">Nepal</asp:ListItem>
                                <asp:ListItem Text="Russia" Value="3">Russia</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvPlace" runat="server" ControlToValidate="DpPlace" InitialValue="0" ErrorMessage="Please select place" ForeColor="Red" ValidationGroup="SR" />
                        </div>
                        <div class="col-xs-12 col-sm-12 col-md-10 col-lg-4 col-xl-4 col-xxl-4">
                            <label for="calDOB" class="form-label">Date of Birth:</label>
                            <asp:TextBox ID="txtDOB" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDOB" runat="server" ControlToValidate="txtDOB" ErrorMessage="Date of Birth is required" ForeColor="Red" ValidationGroup="SR" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 col-md-10 col-lg-2 col-xl-2 col-xxl-2"></div>
                        <div class="col-xs-12 col-sm-12 col-md-10 col-lg-4 col-xl-4 col-xxl-4">
                            <label class="form-label">Gender:</label>
                            <asp:RadioButtonList ID="rblGender" runat="server" CssClass="form-check" Required="true">
                                <asp:ListItem Text="" Value="0" Enabled="false" style="display: none" />
                                <asp:ListItem Text="Male" Value="1" />
                                <asp:ListItem Text="Female" Value="2" />
                                <asp:ListItem Text="Others" Value="3" />
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="rfvGender" runat="server" ControlToValidate="rblGender" InitialValue="" ErrorMessage="Please select your gender" ForeColor="Red" ValidationGroup="SR" />
                        </div>
                        <div class="col-xs-12 col-sm-12 col-md-10 col-lg-4 col-xl-4 col-xxl-4">
                            <label for="filePhoto" class="form-label">Upload Photo:</label>
                            <asp:FileUpload ID="filePhoto" runat="server" CssClass="form-control" OnChange="previewImage()" />
                            <asp:RequiredFieldValidator ID="rfvPhoto" runat="server" ControlToValidate="filePhoto" ErrorMessage="Please upload a photo" ForeColor="Red" ValidationGroup="SR" />
                        </div>
                        <img id="imgPreview" src="" alt="Image Preview" style="display: none; max-width: 100px; margin-top: 0px; border: solid 2px; border-radius: 30px;" />
                        <div class="col-xs-12 col-sm-12 col-md-10 col-lg-2 col-xl-2 col-xxl-2"></div>
                    </div>
                    <div class="mt-3 text-center">
                        <asp:Button ID="btnSubmit" runat="server" Text="Register" CssClass="btn btn-primary" OnClick="btnSubmit_Click" ValidationGroup="SR" />
                    </div>
                </div>
            </div>
        </div>
        <div class="container mt-3">
            <div class="row justify-content-center mt-3">
                <div class="col-md-8">
                    <div class="grid-container">
                        <asp:GridView ID="gvEmployees" runat="server" CssClass="table table-striped table-bordered text-center" AutoGenerateColumns="False" 
                            OnRowEditing="gvEmployees_RowEditing" OnRowCancelingEdit="gvEmployees_RowCancelingEdit" OnRowUpdating="gvEmployees_RowUpdating" 
                            OnRowDeleting="gvEmployees_RowDeleting" OnRowDataBound="gvEmployees_RowDataBound" DataKeyNames="EmpId">
                            <Columns>
                                <asp:BoundField DataField="Empid" HeaderText="Empid" SortExpression="Empid" ReadOnly="true" />
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEmpidEdit" runat="server" Text='<%# Bind("Empid") %>' ReadOnly="true" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="EmpName" HeaderText="EmpName" SortExpression="EmpName" ReadOnly="true" />
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEmpNameEdit" runat="server" Text='<%# Bind("EmpName") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Gender" HeaderText="Gender" SortExpression="Gender" ReadOnly="true" />
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:RadioButtonList ID="rblGenderEdit" runat="server" SelectedValue='<%# Bind("Gender") %>'>
                                            <asp:ListItem Text="Male" Value="Male" />
                                            <asp:ListItem Text="Female" Value="Female" />
                                            <asp:ListItem Text="Others" Value="Others" />
                                        </asp:RadioButtonList>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DOB" HeaderText="DOB" SortExpression="DOB" ReadOnly="true" />
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDOBEdit" runat="server" Text='<%# Bind("DOB") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Place" HeaderText="Place" SortExpression="Place" ReadOnly="true" />
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlPlaceEdit" runat="server" SelectedValue='<%# Bind("Place") %>'>
                                            <asp:ListItem Text="India" Value="India">India</asp:ListItem>
                                            <asp:ListItem Text="Nepal" Value="Nepal">Nepal</asp:ListItem>
                                            <asp:ListItem Text="Russia" Value="Russia">Russia</asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Image">
                                    <ItemTemplate>
                                        <asp:Image ID="imgPhoto" runat="server" Width="50" Height="50" ImageUrl='<%# "data:image/jpeg;base64," + Convert.ToBase64String(Eval("Photo") as byte[]) %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:FileUpload ID="fileUploadEdit" runat="server" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <!-- Bootstrap 5 JS -->
        <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.6/dist/umd/popper.min.js"></script>
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.min.js"></script>
        <script>
            // Bootstrap custom form validation
            (function () {
                'use strict'
                var forms = document.querySelectorAll('.needs-validation')

                Array.prototype.slice.call(forms)
                    .forEach(function (form) {
                        form.addEventListener('submit', function (event) {
                            if (!form.checkValidity()) {
                                event.preventDefault()
                                event.stopPropagation()
                            }
                            form.classList.add('was-validated')
                        }, false)
                    })
            })()

            function previewImage() {
                debugger;
                var fileInput = document.getElementById("filePhoto");
                var imgPreview = document.getElementById("imgPreview");

                if (fileInput.files && fileInput.files[0]) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        imgPreview.src = e.target.result;
                        imgPreview.style.display = "block";
                    }
                    reader.readAsDataURL(fileInput.files[0]);
                } else {
                    imgPreview.src = "";
                    imgPreview.style.display = "none";
                }
            }
    </script>
    </form>
</body>
</html>
