using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsPersonBusiness
    {

        public int PersonID { get; private set; }

        public string NationalNo { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string ThirdName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public byte Gendor { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public int NationalityCountryID { get; set; }

        public string ImagePath { get; set; }

        public string LastImagePath { get; private set; }

        public enum enMode
        {
            AddNew,
            Update
        }

        public enMode _Mode;


        public clsPersonBusiness()
        {
            this.PersonID = -1;
            this.NationalNo = "";
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.Now.AddYears(-21);
            this.Gendor = 0;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.NationalityCountryID = -1;
            this.ImagePath = "";
            this.LastImagePath = "";


            _Mode = enMode.AddNew;

        }


        private bool _AddNewPerson()
        {
            this.PersonID = clsPersonData.AddNewPerson(NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, LastImagePath);

            return (this.PersonID != -1);
        }

        private bool _UpdatePerson()
        {   
            // update person based on his id
            // send object data to the Data Layer
            return clsPersonData.UpdatePerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, LastImagePath);
        }

        static public DataTable ListPeople()
        {
            return clsPersonData.ListPeople();
        }


        static public DataTable FilterPeopleByID(int PersonID)
        {
            return clsPersonData.FilterPeopleByID(PersonID);

        }

        static public DataTable FilterPeopleByNationalNo(string NationalNo)
        {
            return clsPersonData.FilterPeopleByNationalNo(NationalNo);

        }


        static public DataTable FilterPeopleByGendor(byte Gendor)
        {
            return clsPersonData.FilterPeopleByGendor(Gendor);

        }      
        
        static public DataTable FilterPeople(string ColumnName, string filter)
        {
            return clsPersonData.FilterPeople(ColumnName, filter);

        }

        static public bool IsNationalNoUnique (string NationalNo)
        {
            return clsPersonData.IsNationalNoUnique(NationalNo);
        }

        static public bool DeletePerson(int PersonID)
        {
            return clsPersonData.DeletePerson(PersonID);
        }


        public bool FindPersonByID(int PersonID)
        {
            // Retrieve the DataTable from the data access layer
            DataTable dt = clsPersonData.FilterPeopleByID(PersonID);

            // Check if dt is null or contains no rows
            if (dt == null || dt.Rows.Count == 0)
            {
                Console.WriteLine($"Person with ID {PersonID} not found (Business Layer FindPersonByID).");
                return false;
            }

            // Assuming there's always at least one row if we reach this point
            DataRow row = dt.Rows[0];

            // Assigning the data to the properties
            this.PersonID = PersonID;
            this.NationalNo = row["NationalNo"].ToString();
            this.FirstName = row["FirstName"].ToString();
            this.SecondName = row["SecondName"].ToString();
            this.ThirdName = row["ThirdName"] != DBNull.Value ? row["ThirdName"].ToString() : null;
            this.LastName = row["LastName"].ToString();
            this.DateOfBirth = row["DateOfBirth"] != DBNull.Value ? Convert.ToDateTime(row["DateOfBirth"]) : DateTime.MinValue;
            this.Gendor = row["Gendor"] != DBNull.Value ? Convert.ToByte(row["Gendor"]) : (byte)0;
            this.Address = row["Address"].ToString();
            this.Phone = row["Phone"].ToString();
            this.Email = row["Email"] != DBNull.Value ? row["Email"].ToString() : null;
            this.NationalityCountryID = row["NationalityCountryID"] != DBNull.Value ? Convert.ToInt32(row["NationalityCountryID"]) : 0;
            this.ImagePath = row["ImagePath"] != DBNull.Value ? row["ImagePath"].ToString() : null;

            this._Mode = enMode.Update;

            return true;
        }


        public bool FindPersonByNationalNo(string NationalNo)
        {
            DataTable dt = clsPersonData.FilterPeopleByNationalNo(NationalNo);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                this.PersonID = Convert.ToInt32(row["PersonID"]);
                this.NationalNo = row["NationalNo"].ToString();
                this.FirstName = row["FirstName"].ToString();
                this.SecondName = row["SecondName"].ToString();
                this.ThirdName = row["ThirdName"] != DBNull.Value ? row["ThirdName"].ToString() : null;
                this.LastName = row["LastName"].ToString();
                this.DateOfBirth = row["DateOfBirth"] != DBNull.Value ? Convert.ToDateTime(row["DateOfBirth"]) : DateTime.MinValue;
                this.Gendor = row["Gendor"] != DBNull.Value ? Convert.ToByte(row["Gendor"]) : (byte)0;
                this.Address = row["Address"].ToString();
                this.Phone = row["Phone"].ToString();
                this.Email = row["Email"] != DBNull.Value ? row["Email"].ToString() : null;

                this.NationalityCountryID = row["NationalityCountryID"] != DBNull.Value ? Convert.ToInt32(row["NationalityCountryID"]) : 0;

                this.ImagePath = row["ImagePath"] != DBNull.Value ? row["ImagePath"].ToString() : null;

                this._Mode = enMode.Update;
                return true;
            }
            else
            {
                Console.WriteLine($"Person With NationalNo: {NationalNo} Not Fround (Business Layer FindPersonByNationalNo).");
                return false;
            }
        }


        private void HandleImagePath()
        {
   
            string DirectoryPath = Path.GetDirectoryName(ImagePath);

            Guid Guid = Guid.NewGuid();

            string DestinationPath = $"D:\\programming\\DVLD_Photos\\{Guid}.png";

            if (DirectoryPath != "C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources" && DirectoryPath != null)
            {
                File.Copy(ImagePath, DestinationPath);

                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(LastImagePath) && File.Exists(LastImagePath))
                {
                    try
                    {
                        File.Delete(LastImagePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                // Update the last image path
                LastImagePath = DestinationPath;


            }
            else
            {
                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(LastImagePath) && File.Exists(LastImagePath))
                {
                    try
                    {
                        File.Delete(LastImagePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                // Update the last image path
                LastImagePath = "";
            }


        }

        public bool SavePersonToDB()
        {

            HandleImagePath();
            //Console.WriteLine("Mode Is" + _Mode);

            switch (_Mode)
            {

                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                        _Mode = enMode.Update;
                        return true; 

                    }
                    else
                    {
                        Console.WriteLine("Failed To Add New Person (Business Layer SavePersonToDB Method).");
                        return false;
                    }


                case enMode.Update:

                    if(_UpdatePerson())
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Failed To Update Person (Business Layer SavePersonToDB Method).");

                        return false;
                    }



                default:
                    Console.WriteLine("Invalid mode encountered in SavePersonToDB method.");
                    return false;

            }
            
        }


        public static string GetFullNameUsingPersonID(int PersonID)
        {
            return clsPersonData.GetFullNameUsingPersonID(PersonID);
        }


        public static int GetPersonIDUsingNationalNo(string NationalNo)
        {
            return clsPersonData.GetPersonIDUsingNationalNo(NationalNo);

        }

        public static string GetPersonNationalNoUsingPerosnID(int PersonID)
        {
            return clsPersonData.GetPersonNationalNoUsingPerosnID(PersonID);
        }
        public static bool GetPersonGendorUsingPersonID(int PersonID)
        {
            return clsPersonData.GetPersonGendorUsingPersonID(PersonID);
        }


    }
}
