using System;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using SCADA911;
using System.Xml;
using System.Data;
using System.Text;

namespace SCADA911Lib
{
    public partial class frmUsers : Form
    {
        private string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); //Gets Application Data Path
        ArrayList userList = new ArrayList();
        //private User userObj = new User();
        public frmUsers()
        {
            InitializeComponent();
        }

        private void frmUsers_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(appDataPath + "//SCADA911//Users"))
                Directory.CreateDirectory(appDataPath + "//SCADA911//Users"); //Create Directory

            if (!File.Exists(appDataPath + "//SCADA911//Users//UserList.xml"))//Create File
            {
                XmlTextWriter xWriter = new XmlTextWriter(appDataPath + "//SCADA911//Users//UserList.xml", Encoding.UTF8);
                xWriter.WriteStartElement("Users");
                xWriter.WriteEndElement();
                xWriter.Close();
            }
            xmlLoad();
          
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            User userObj = new User();
            userObj.Name = txtName.Text;
            userObj.Phone1 = txtPhone1.Text;
            userObj.Phone2 = txtPhone2.Text;
            userObj.Email = txtEmail.Text;
            userObj.SMSPhone = txtSMS.Text;
            foreach (User userObj1 in userList)
            {
                if (userObj.Name == userObj1.Name)
                {
                    MessageBox.Show("Sorry this name already exists.", "Important Message", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    break;
                }
            }
            userList.Add(userObj);
            clearTextbox();
            lstUser.Items.Add(userObj.Name);
        }
        private void clearTextbox()
        {
            txtName.Text = "";
            txtPhone1.Text = "";
            txtPhone2.Text = "";
            txtEmail.Text = "";
            txtSMS.Text = "";
        }

        private void frmUsers_FormClosing(object sender, FormClosingEventArgs e)
        {
           // xmlSave();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            xmlSave();
        }

        private void lstUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            User userObj = new User();
            if (!(lstUser.SelectedIndex==-1))
            {
                userObj = (User)userList[lstUser.SelectedIndex];
                txtName.Text = userObj.Name;
                txtPhone1.Text = userObj.Phone1;
                txtPhone2.Text = userObj.Phone2;
                txtEmail.Text = userObj.Email;
                txtSMS.Text = userObj.SMSPhone;
            }

        }
        private void xmlSave()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(appDataPath + "//SCADA911//Users//UserList.xml");
            XmlNode xNode = xDoc.SelectSingleNode("Users");
            foreach (User userObj in userList)
            {
                XmlNode xTop = xDoc.CreateElement("User");
                XmlNode xName = xDoc.CreateElement("Name");
                XmlNode xPhone1 = xDoc.CreateElement("Phone1");
                XmlNode xPhone2 = xDoc.CreateElement("Phone2");
                XmlNode xEmail = xDoc.CreateElement("Email");
                XmlNode xSMS = xDoc.CreateElement("SMS");
                xName.InnerText = userObj.Name;
                xPhone1.InnerText = userObj.Phone1;
                xPhone2.InnerText = userObj.Phone2;
                xEmail.InnerText = userObj.Email;
                xSMS.InnerText = userObj.SMSPhone;
                xTop.AppendChild(xName);
                xTop.AppendChild(xPhone1);
                xTop.AppendChild(xPhone2);
                xTop.AppendChild(xEmail);
                xTop.AppendChild(xSMS);
                xDoc.DocumentElement.AppendChild(xTop);
            }
            xDoc.Save(appDataPath + "//SCADA911//Users//UserList.xml");
        }
        private void xmlLoad()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(appDataPath + "//SCADA911//Users//UserList.xml");
            XmlNode xNode = xDoc.SelectSingleNode("Users");
            User userObj = new User();
            foreach (XmlNode xNodeNew in xDoc.SelectNodes("Users/User"))
            {
                userObj.Name = xNodeNew.SelectSingleNode("Name").InnerText;
                userObj.Phone1 = xNodeNew.SelectSingleNode("Phone1").InnerText;
                userObj.Phone2 = xNodeNew.SelectSingleNode("Phone2").InnerText;
                userObj.Email = xNodeNew.SelectSingleNode("Email").InnerText;
                userObj.SMSPhone = xNodeNew.SelectSingleNode("SMS").InnerText;
                userList.Add(userObj);
                lstUser.Items.Add(userObj.Name);
            }
        }

    }
}
