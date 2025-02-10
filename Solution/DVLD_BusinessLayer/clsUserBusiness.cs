using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsUserBusiness
    {

        public int UserID { get; set; }

        public int PersonID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public enum enMode
        {
            AddNew,
            Update
        }

        public enMode _Mode;
        public clsUserBusiness() 
        {
            this.UserID = -1;
            this.PersonID = -1;
            this.UserName = "user";
            this.Password = "0000";
            this.IsActive = false;

            _Mode = enMode.AddNew;

        }
        private bool _AddNewUser()
        {
            this.UserID = clsUserData.AddNewUser(PersonID, UserName, Password, IsActive);

            return (this.UserID != -1);
        }

        private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(UserID, UserName, Password, IsActive);
        }


        // call on instance of this class
        public bool ValidateUser(string UserName, string Password) 
        {

            DataRow dr = clsUserData.FindUserUsingPasswordAndUserName(UserName, Password);

            if (dr != null)
            {
                this.UserID = (int) dr["UserID"];
                this.PersonID = (int) dr["PersonID"];
                this.UserName = (string) dr["UserName"];
                this.Password = (string) dr["Password"];
                this.IsActive = (bool) dr["IsActive"];

                if (this.IsActive)
                {
                    return true;    

                } else
                {
                    Console.WriteLine("User Is Not Active (ValidateUser) clsUserBusines ");
                    return true;
                }
            } 
            else
            {
                Console.WriteLine("dr = null (ValidateUser) clsUserBusines ");
                return false;
            }
        }


        static public DataTable ListUsers()
        {
            return clsUserData.ListUsers();
        }

        static public DataTable FilterUsers(string ColumnName, string filter)
        {
            return clsUserData.FilterUsers(ColumnName, filter);

        }

        static public DataTable FilterUsers(string ColumnName, int ID)
        {
            return clsUserData.FilterUsers(ColumnName, ID);

        }

        static public DataTable FilterUsers(string ColumnName, bool IsActive)
        {
            return clsUserData.FilterUsers(ColumnName, IsActive);

        }

        static public bool IsPersonAUser(int PerosnID)
        {
            return clsUserData.IsPersonAUser(PerosnID);
        }

        static public clsUserBusiness FindUserByID(int UserID)
        {
            DataTable dt = clsUserData.FindUserByID(UserID);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                clsUserBusiness User = new clsUserBusiness();

                User.UserID = UserID;
                User.PersonID = (int)row["PersonID"];
                User.UserName = row["UserName"].ToString();
                User.Password = row["Password"].ToString();
                User.IsActive = (bool)row["IsActive"];


                User._Mode = enMode.Update;
                return User;
            }
            else
            {
                Console.WriteLine($"User With ID {UserID} Not Fround (Business Layer FindUserByID).");
                return null;
            }
        }


        static public clsUserBusiness FindUserUsingPasswordAndUserName(string UserName, string Password)
        {

            DataRow dr = clsUserData.FindUserUsingPasswordAndUserName(UserName, Password);

            if (dr != null)
            {

                clsUserBusiness User = new clsUserBusiness();

                User.UserID = (int)dr["UserID"];
                User.PersonID = (int)dr["PersonID"];
                User.UserName = (string)dr["UserName"];
                User.Password = (string)dr["Password"];
                User.IsActive = (bool)dr["IsActive"];
                return User;    

                
            }
            else
            {
                Console.WriteLine("dr = null (FindUserUsingPasswordAndUserName) clsUserBusines ");
                return null;
            }
        }


        static public int FindUserIDUsingPasswordAndUserName(string UserName, string Password)
        {

            return clsUserData.FindUserIDUsingPasswordAndUserName(UserName, Password);
            
        }


        public bool SaveUserToDB()
        {
            switch (_Mode)
            {

                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        _Mode = enMode.Update;
                        return true;

                    }
                    else
                    {
                        Console.WriteLine("Failed To Add New User (Business Layer SaveUserToDB Method).");
                        return false;
                    }


                case enMode.Update:

                    if (_UpdateUser())
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Failed To Update User (Business Layer SaveUserToDB Method).");

                        return false;
                    }



                default:
                    Console.WriteLine("Invalid mode encountered in SaveUserToDB method.");
                    return false;

            }
        }   


        static public bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID);
        }

        public static string FindUserNameUsingUserID(int UserID)
        {
            return clsUserData.FindUserNameUsingUserID(UserID);
        }


    }
}
