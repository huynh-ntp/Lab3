using System;
using System.Windows.Forms;
using System.IO;

namespace EmployeeForm
{
    public partial class EmployeeUI : Form
    {
        //String EmpID;
        bool showSearch = false;
        int rowIndex = -1;
        public EmployeeUI()
        {      
            InitializeComponent();
            BtnAdd.Enabled = false;
            BtnDelete.Enabled = false;
            BtnUpdate.Enabled = false;
        }     
        bool ValidateInput()
        {
            errorProvider1.Clear();
            string name = txtFullName.Text.Trim();
            bool bError = false;
            if (name.Length == 0)
            {   
                errorProvider1.SetError(txtFullName, "Please enter your name!");
                bError = true;
            }
            
            
            DateTime currDate = DateTime.Now;
            int currYear = currDate.Year;
            DateTime DateofBirth = dTPDOB.Value;
            int birthYear = DateofBirth.Year;
            if (currYear - birthYear < 18)
            {
                errorProvider1.SetError(dTPDOB,
                    "Age must be greater than or equal to 18");
                bError = true;
            }
            if (radMale.Checked == false && radFemale.Checked == false)
            {
                errorProvider1.SetError(groupBox1, "Please select gender!");
                bError = true;
            }
            if (mtxtPhone.MaskCompleted == false)
            {
                errorProvider1.SetError(mtxtPhone,
                    "please enter required digit!");
                bError = true;
            }

            string national = cbNational.Text;

            if ((!cbNational.Items.Contains(national))){
                if (national.Trim().Length < 0)
                {
                    errorProvider1.SetError(cbNational,
                        "please select National!");
                    bError = true;
                }
                else
                {
                    cbNational.Items.Add(national);
                }
                
            }
            else
            {
                if (cbNational.SelectedIndex < 0)
                {
                    errorProvider1.SetError(cbNational,
                        "please select National!");
                    bError = true;
                }
            }

            try
            {
                int salary = Int32.Parse(txtSalary.Text);
                if (salary <= 0)
                {
                    errorProvider1.SetError(txtSalary, "Salary must be greater than 0!");
                    bError = true;
                }
            }
            catch ( Exception e)
            {
                errorProvider1.SetError(txtSalary, "Salary must be number!");
                bError = true;
            }
            
            if (bError == true)
            {
                return false;
            }
            else
                errorProvider1.Clear();
            return true;
        }
        void AddNewEmployee()
        {
            string gender = "";
            if (radMale.Checked == true)
                gender = "Male";
            else if (radFemale.Checked == true) gender = "Female";

            DataGridViewRow r = new DataGridViewRow();
            r.CreateCells(dgEmployees);
            r.SetValues(txtFullName.Text,
                dTPDOB.Value.ToShortDateString(),
                gender, cbNational.Text, mtxtPhone.Text, txtAddress.Text,
                cbQualification.Text, txtSalary.Text);
            dgEmployees.Rows.Add(r);
            
            
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            //reset
            txtFullName.Clear();
            txtAddress.Clear();
            txtSalary.Clear();
            mtxtPhone.Clear();
            dTPDOB.Value = DateTime.Now;
            radFemale.Checked = false;
            radMale.Checked = false;
            cbNational.SelectedIndex = -1;
            cbQualification.SelectedIndex = -1;
            //Active/Deactive
            BtnAdd.Enabled = true;
            BtnDelete.Enabled = false;
            BtnUpdate.Enabled = false;
            txtFullName.Focus();
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInput() == false)
                return;
            AddNewEmployee();
            BtnAdd.Enabled = false;
            MessageBox.Show("Add success!");
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SearchbyName();
        }
        void SearchbyName()
        {
            SearchResultui frm = new SearchResultui();
            for (int i = 0; i < dgEmployees.Rows.Count; i++)
            {
                DataGridViewRow r = dgEmployees.Rows[i];
                if (r.Cells[0].Value.ToString().Equals(txtName.Text))
                {
                    frm.AddEmployeeInfo(r.Cells[0].Value.ToString(),
                    r.Cells[1].Value.ToString(), r.Cells[2].Value.ToString(),
                    r.Cells[3].Value.ToString(), r.Cells[4].Value.ToString(),
                    r.Cells[5].Value.ToString(), r.Cells[6].Value.ToString(),
                    r.Cells[7].Value.ToString());
                }
            }
            frm.ShowDialog();
        }
        private void BtnFilter_Click(object sender, EventArgs e)
        {
            SearchResultui frm = new SearchResultui();
            for (int i = 0; i < dgEmployees.Rows.Count; i++)
            {
                DataGridViewRow r = dgEmployees.Rows[i];
                if (r.Cells[3].Value.ToString().Equals(cbNationalFilter.Text))
                //if (r.Cells[3].Value.ToString().Equals(cbNationalFilter.Text))
                {
                    frm.AddEmployeeInfo(r.Cells[0].Value.ToString(),
                    r.Cells[1].Value.ToString(), r.Cells[2].Value.ToString(),
                    r.Cells[3].Value.ToString(), r.Cells[4].Value.ToString(),
                    r.Cells[5].Value.ToString(), r.Cells[6].Value.ToString(),
                    r.Cells[7].Value.ToString());
                }
            }
            frm.ShowDialog();
        }
        private void BtnSavetoFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            // saveFileDialog1.Filter = "Data files|*.dat|Text files|*.txt|Both files|*.dat;*.txt";
            DialogResult rs = saveFileDialog1.ShowDialog();
            if (rs == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName;
                StreamWriter sw = new StreamWriter(fileName);
                string Line = "";
                for (int i = 0; i < dgEmployees.Rows.Count - 0; ++i)
                {
                    if (i == dgEmployees.NewRowIndex)
                    {
                        break;
                    }
                    Line = "";
                    for (int j = 0; j < dgEmployees.Columns.Count; ++j)
                    {
                        Line += dgEmployees[j, i].Value + ";";
                    }
                    //sw.WriteLine();
                    sw.WriteLine(Line);
                }
                sw.Close();
                MessageBox.Show("Save success!");
            }
        }
        private void BtnLoadfromFile_Click(object sender, EventArgs e)
        {
            //openFileDialog1.Filter = "Data files|*.dat|Text files|*.txt||*.dBoth filesat;*.txt";
            openFileDialog1.Filter = "Data files|*.dat|Text files|*.txt|Both files|*.dat;*.txt|All files|*.*";
            //OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult rs = openFileDialog1.ShowDialog();
            if (rs == DialogResult.OK)
            {
                dgEmployees.Rows.Clear();
                string fileName = openFileDialog1.FileName;
                StreamReader sr = new StreamReader(fileName);
                string Line = sr.ReadLine();
                while ((Line != null) && (Line != " "))
                {
                    string[] array = Line.Split(';');
                    dgEmployees.Rows.Add(array);
                    Line = sr.ReadLine();
                }
                sr.Close();
            }
        }
        private void BtnShowHide_Click(object sender, EventArgs e)
        {
            pnSearchOptions.Visible = showSearch;
            showSearch = !showSearch;
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void dgEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            rowIndex = e.RowIndex;
            BtnUpdate.Enabled = true;
            BtnDelete.Enabled = true;
            BtnAdd.Enabled = false;
            if (rowIndex >= 0)
            {
                DataGridViewRow row = dgEmployees.Rows[rowIndex];
                txtFullName.Text = row.Cells[0].Value.ToString();
                dTPDOB.Value = Convert.ToDateTime(row.Cells[1].Value.ToString());
                String gender = row.Cells[2].Value.ToString();
                if (gender.Equals("Male"))
                {
                    radMale.Checked = true;
                }
                else
                {
                    radFemale.Checked = true;
                }
                String national = row.Cells[3].Value.ToString();
                if (cbNational.Items.Contains(national))
                {
                    cbNational.SelectedItem = national;
                }
                else
                {
                    cbNational.Items.Add(national);
                    cbNational.SelectedItem = national;
                }
                String phone = row.Cells[4].Value.ToString();
                mtxtPhone.Text = phone;
                String address = row.Cells[5].Value.ToString();
                txtAddress.Text = address;
                String qualification = row.Cells[6].Value.ToString();
                cbQualification.SelectedItem = qualification;
                String salary = row.Cells[7].Value.ToString();
                txtSalary.Text = salary;            
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (ValidateInput() == false)
                return;
            UpdateEmployee();
        }

        void UpdateEmployee()
        {
            if (rowIndex >= 0)
            {               
                string gender = "";
                if (radMale.Checked == true)
                    gender = "Male";
                else if (radFemale.Checked == true) gender = "Female";
                dgEmployees.Rows[rowIndex].SetValues(txtFullName.Text,
                dTPDOB.Value.ToShortDateString(),
                gender, cbNational.Text, mtxtPhone.Text, txtAddress.Text,
                cbQualification.Text, txtSalary.Text);
                MessageBox.Show("Update success!");
                
            }
            else
            {
                return;
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (rowIndex >= 0)
            {
                if(MessageBox.Show("Really delete?","Confirm delete",MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DataGridViewRow row = dgEmployees.Rows[rowIndex];
                    dgEmployees.Rows.Remove(row);
                    MessageBox.Show("Delete success!");
                    BtnUpdate.Enabled = false;
                    BtnDelete.Enabled = false;
                }
            }
        }
    }
}
