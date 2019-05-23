using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;


namespace GambiaReview.Models
{
    public class SecurityFunctions
    {
        //Check if email exist in database
        public bool ValidateEmail(string email)
        {
            if (!string.IsNullOrEmpty(email) && IsValidEmail(email))
            {
                using (var db = new DBConnection())
                {
                    if(db.Accounts.Any(s=> s.Email == email))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //Get the Salt for provided Email
        public static string GetEmailSalt(string email)
        {
            if (!string.IsNullOrEmpty(email) && IsValidEmail(email))
            {
                using (var db = new DBConnection())
                {
                    if (db.Accounts.Any(s => s.Email == email))
                    {
                        return db.Accounts.Where(s => s.Email == email).FirstOrDefault().Salt;
                    }
                }
            }
            return null;
        }


        public static string SplitString(string string_text, int return_part)
        {
            if (!string.IsNullOrEmpty(string_text))
            {
                if (string_text.Length % 2 == 0)
                {
                    int half_length = string_text.Length / 2;
                    if(return_part == 0)
                    {
                        return string_text.Substring(0, half_length);
                    }
                    return string_text.Substring(half_length, (string_text.Length - half_length));
                }
                else
                {
                    double string_length = string_text.Length / 2;
                    int half_length = Convert.ToInt32(Math.Ceiling(string_length));
                    if (return_part == 0)
                    {
                        return string_text.Substring(0, half_length);
                    }
                    return string_text.Substring(half_length, (string_text.Length - half_length));
                }
            }
            return null;
        }


        //Concat password string and Hash
        public static string FormatPassword(string pw1, string pw2, string salt1, string salt2)
        {
            string text_password = pw1 + salt1 + pw2 + salt2;
            return GetStringSha256Hash(text_password);
        }

        // ReturnHash Password
        public static string ReturnHashPassword(string password, string email)
        {
            string salt = GetEmailSalt(email);
            string pw1 = SplitString(password, 0);
            string pw2 = SplitString(password, 1);
            string salt1 = SplitString(salt, 0);
            string salt2 = SplitString(salt, 1);
            return FormatPassword(pw1, pw2, salt1, salt2);
        }

        public static string ReturnHashPassword(string password, string email, string salt)
        {
            string pw1 = SplitString(password, 0);
            string pw2 = SplitString(password, 1);
            string salt1 = SplitString(salt, 0);
            string salt2 = SplitString(salt, 1);
            return FormatPassword(pw1, pw2, salt1, salt2);
        }

        /// <summary>
        ///Hash string in c#
        /// </summary>
        ///https://stackoverflow.com/questions/3984138/hash-string-in-c-sharp
        /// <returns></returns>
        internal static string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }

        //SHA256 Hashing for Sessions
        //https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/
        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        //Return Account Status
        public int ReturnAccountStatus(string email)
        {
            if (!string.IsNullOrEmpty(email) && IsValidEmail(email))
            {
                using (var db = new DBConnection())
                {
                    if (db.Accounts.Any(s => s.Email == email))
                    {
                        return (int)db.Accounts.Where(s => s.Email == email).FirstOrDefault().Status;
                    }
                }
            }
            return 0;
        }

        //Check if Login is Valid
        public bool IsLoginValid(string email, string password)
        {
            if ((!string.IsNullOrEmpty(email) && IsValidEmail(email)) && !string.IsNullOrEmpty(password))
            {
                string hashed_password = ReturnHashPassword(password, email); //Hash password
                using (var db = new DBConnection())
                {
                    if (db.Accounts.Any(s => s.Email == email && s.Password == hashed_password))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //Check if Account is Locked
        public bool IsAccountLocked(string email)
        {
            if (!string.IsNullOrEmpty(email) && IsValidEmail(email))
            {
                using (var db = new DBConnection())
                {
                    if (db.LoginInfo.Any(s => s.Email == email && s.LockedStatus == 1))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //Return Account Data
        public string ReturnAccountData(string email, string column)
        {
            if (!string.IsNullOrEmpty(email) && IsValidEmail(email))
            {
                using (var db = new DBConnection())
                {
                    if (db.Accounts.Any(s => s.Email == email))
                    {
                        if (column == "ID")
                        {
                            return db.Accounts.Where(s => s.Email == email).FirstOrDefault().ID.ToString();
                        }
                        else if(column == "FirstName")
                        {
                            return db.Accounts.Where(s => s.Email == email).FirstOrDefault().FirstName;
                        }
                        else if (column == "LastName")
                        {
                            return db.Accounts.Where(s => s.Email == email).FirstOrDefault().LastName;
                        }
                        else if (column == "Country")
                        {
                            return db.Accounts.Where(s => s.Email == email).FirstOrDefault().Country;
                        }
                        else if (column == "CountryCode")
                        {
                            return ((int)db.Accounts.Where(s => s.Email == email).FirstOrDefault().CountryCode).ToString();
                        }
                        else if (column == "PhoneNumber")
                        {
                            return db.Accounts.Where(s => s.Email == email).FirstOrDefault().PhoneNumber;
                        }
                        else if (column == "Status")
                        {
                            return db.Accounts.Where(s => s.Email == email).FirstOrDefault().Status.ToString();
                        }
                        else if (column == "Oauth")
                        {
                            return db.Accounts.Where(s => s.Email == email).FirstOrDefault().Oauth.ToString();
                        }
                        else if (column == "AccountVerification")
                        {
                            return db.Accounts.Where(s => s.Email == email).FirstOrDefault().AccountVerification.ToString();
                        }
                        else if (column == "DirectoryName")
                        {
                            return db.Accounts.Where(s => s.Email == email).FirstOrDefault().DirectoryName;
                        }
                        else if (column == "Password")
                        {
                            return db.Accounts.Where(s => s.Email == email).FirstOrDefault().Password;
                        }
                        else if (column == "Salt")
                        {
                            return db.Accounts.Where(s => s.Email == email).FirstOrDefault().Salt;
                        }
                        else if (column == "DateCreated")
                        {
                            return ((DateTime)db.Accounts.Where(s => s.Email == email).FirstOrDefault().DateCreated).ToString();
                        }
                    }
                }
            }
            return null;
        }


        //Add new User
        public static bool AddNewUserRegistration(string email, string password)
        {
            bool process_status = false;
            using (var db = new DBConnection())
            {
                string salt = AppFunctions.RandomString(GetRandomSaltLength());
                string hashed_password = ReturnHashPassword(password, email, salt);
                string[] directory_arr = email.Split(new[] { '@' });
                string directory_name = directory_arr[0]; 
                //If name already exist, add random number to name
                if(db.Accounts.Any(s=> s.DirectoryName == directory_name))
                {
                    directory_name = directory_name + AppFunctions.RandomInt(4);
                }
                AccountsModel AccountData = new AccountsModel
                {
                    Email = email,
                    Password = hashed_password,
                    Salt = salt,
                    Status = 1,
                    AccountVerification = 0,
                    DirectoryName = directory_name,
                    DateCreated = DateTime.Now
                    // …
                };

                db.Accounts.Add(AccountData);

                try
                {
                    db.SaveChanges();
                    process_status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    //Log Error
                    LogError(ex, null, "AddNewUserRegistration", null);
                }

            }
            return process_status;
        }

        //Add New Registration
        public static bool AddNewRegistration(string email, string password, bool external)
        {
            bool process_status = false;

            int oauth = 0;
            int account_verified = 0;
            if (external)
            {
                password = ShuffleString(password);
                oauth = 1;
                account_verified = 1;
            }

            string salt = AppFunctions.RandomString(GetRandomSaltLength());
            string hashed_password = ReturnHashPassword(password, email, salt);
            string directory_name = AppFunctions.GetUsernameFromEmail(email);
            //If name already exist, add random number to name
            using (var db = new DBConnection())
            {
                if (db.Accounts.Any(s => s.DirectoryName == directory_name))
                {
                    directory_name = directory_name + AppFunctions.RandomInt(4);
                }
            }

            //Create directory
            //System.IO.Directory.CreateDirectory("~/images/account");

            string connString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    //Insert record to Users db
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO Accounts ([Email], [Password], [Salt], [Status], [Oauth], [AccountVerification], [DirectoryName], [DateCreated]) 
                                            VALUES 
                                    (@var0, @var1, @var2, @var3, @var4, @var5, @var6, @var7)";
                    cmd.Parameters.AddWithValue("@var0", email);
                    cmd.Parameters.AddWithValue("@var1", hashed_password);
                    cmd.Parameters.AddWithValue("@var2", salt);
                    cmd.Parameters.AddWithValue("@var3", 1);
                    cmd.Parameters.AddWithValue("@var4", oauth);
                    cmd.Parameters.AddWithValue("@var5", account_verified);
                    cmd.Parameters.AddWithValue("@var6", directory_name);
                    cmd.Parameters.AddWithValue("@var7", DateTime.Now);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 1)
                    {
                        process_status = true;
                    }
                    else
                    {
                        process_status = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error! <br>" + ex);
                //Log Error
                LogError(ex, null, "AddNewRegistration", null);
            }
            finally
            {
                if (conn != null)
                {
                    //cleanup connection i.e close 
                    conn.Close();
                }
            }
            return process_status;
        }


        //Add user login data
        public static bool AddLoginInfo(string email, DateTime last_login, int failed_login, int locked_status, DateTime? lock_period, 
                          int total_logins, string session_id, DateTime  first_login)
        {
            bool process_status = false;
            using (var db = new DBConnection())
            {
                //First Check if user has record
                if (db.LoginInfo.Any(s => s.Email == email))
                {
                    //Update record
                    if (UpdateLoginInfoData(email, last_login, failed_login, locked_status, lock_period, total_logins, session_id))
                    {
                        return true;
                    }
                    return false;
                }
                
                LoginInfoModel LoginData = new LoginInfoModel
                {
                    Email = email,
                    LastLogin = last_login,
                    FailedLoginCount = failed_login,
                    LockedStatus = locked_status,
                    LockPeriod = lock_period,
                    TotalLogins = total_logins,
                    LoginSessionID = session_id,
                    FirstLogin = DateTime.Now
                    // …
                };
                
                db.LoginInfo.Add(LoginData);
                
                try
                {
                    db.SaveChanges();
                    process_status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    //ErrorHandler.LogError(DBFunctions.RandomString(15), "New Leave Transaction", Convert.ToString(ex), "Error occured on creating a new leave transaction.", CheckRights.UserIdentityName(RequestedBy), "Models/ComputeLeave.cs");
                }

            }
            return process_status;
        }


        //Update login data
        public static bool UpdateLoginInfoData(string email, DateTime last_login, int failed_login, int locked_status, DateTime? lock_period,
                          int total_logins, string session_id)
        {

            bool process_status = false;
            using (var db = new DBConnection())
            {
                // Query the database for the row to be updated.
                var query =
                    from login_data in db.LoginInfo
                    where login_data.Email == email
                    select login_data;

                // Execute the query, and change the column values
                // you want to change.
                foreach (LoginInfoModel data in query)
                {
                    data.LastLogin =  DateTime.Now;
                    data.FailedLoginCount = failed_login;
                    data.LockedStatus = locked_status;
                    if(lock_period != null)
                    {
                        data.LockPeriod = lock_period;
                    }
                    data.TotalLogins = total_logins;
                    data.LoginSessionID = session_id;
                    // Insert any additional changes to column values.
                }

                // Submit the changes to the database.
                try
                {
                    db.SaveChanges();
                    process_status = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Provide for exceptions.
                }

            }
            return process_status;
        }


        //Update Profile Info
        public static bool UpdateProfileInfo(string email, string first_name, string last_name, string country, int? country_code, string phone_number)
        {
            bool process_status = false;
            using (var db = new DBConnection())
            {
                var query =
                    from account in db.Accounts
                    where account.Email == email
                    select account;

                foreach (AccountsModel account_data in query)
                {
                    account_data.FirstName = first_name;
                    account_data.LastName = last_name;
                    account_data.Country = country;
                    account_data.PhoneNumber = phone_number;
                    if (country_code != null)
                    {
                        account_data.CountryCode = country_code;
                    }
                }
                
                try
                {
                    db.SaveChanges();
                    process_status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    //Log Error
                    LogError(ex, email, "UpdateProfileInfo", null);
                }

            }
            return process_status;
        }

        //Add Profile Info (Not in use)
        public static bool AddProfileData(string email, string first_name, string last_name, string country, int? country_code, int? phone_number)
        {
            bool process_status = false;

            string connString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    //Insert record to Users db
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO Users ([FirstName], [LastName], [Country], [CountryCode], [PhoneNumber]) 
									VALUES 
							(@var0, @var1, @var2, @var3, @var4)";
                    cmd.Parameters.AddWithValue("@var0", ((object)first_name) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@var1", ((object)last_name) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@var2", ((object)country) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@var3", ((object)country_code) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@var4", ((object)phone_number) ?? DBNull.Value);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 1)
                    {
                        process_status = true;
                    }
                    else
                    {
                        process_status = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error! <br>" + ex);
                //Log Error
                LogError(ex, email, "AddProfileData", null);
            }
            finally
            {
                if (conn != null)
                {
                    //cleanup connection i.e close 
                    conn.Close();
                }
            }
            return process_status;
        }

        //Update Profile Info
        public static bool UpdateProfileData(string email, string first_name, string last_name, string country, int? country_code, string phone_number)
        {
            bool success = false;
            string connString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    //Update the record with the new value for status
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"UPDATE [Accounts] SET [FirstName] = @var0, [LastName] = @var1, [Country] = @var2, [CountryCode] = @var3, [PhoneNumber] = @var4 WHERE [email] = @var5";
                    cmd.Parameters.AddWithValue("@var0", ((object)first_name) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@var1", ((object)last_name) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@var2", ((object)country) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@var3", ((object)country_code) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@var4", ((object)phone_number) ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@var5", email);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (conn != null)
                {
                    //cleanup connection i.e close 
                    conn.Close();
                }
            }
            return success;
        }

        //Add Profile Pic
        public static bool AddProfilePic(string email, string profile_pic)
        {
            bool process_status = false;
            using (var db = new DBConnection())
            {
                if (db.ProfilePictures.Any(s=> s.UserEmail == email))
                {
                    if(UpdateProfilePic(email, profile_pic))
                    {
                        return true;
                    }
                    return false;
                }

                ProfilePicturesModel profile = new ProfilePicturesModel
                {
                    UserEmail = email,
                    ProfilePicture = profile_pic,
                    DateAdded = DateTime.Now
                    // …
                };

                db.ProfilePictures.Add(profile);

                try
                {
                    db.SaveChanges();
                    process_status = true;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                    db.SaveChanges();
                }

            }
            return process_status;
        }


        //Update profile pic
        public static bool UpdateProfilePic(string email, string profile_pic)
        {
            bool process_status = false;
            using (var db = new DBConnection())
            {
                var query =
                    from profile in db.ProfilePictures
                    where profile.UserEmail == email
                    select profile;

                foreach (ProfilePicturesModel profile_data in query)
                {
                    profile_data.UserEmail = email;
                    profile_data.ProfilePicture = profile_pic;
                }

                try
                {
                    db.SaveChanges();
                    process_status = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Log Error
                }

            }
            return process_status;
        }

        //Return Account Profile Pic
        public string ReturnAccountProfilePic(string email)
        {
            if (!string.IsNullOrEmpty(email) && IsValidEmail(email))
            {
                using (var db = new DBConnection())
                {
                    if (db.ProfilePictures.Any(s => s.UserEmail == email))
                    {
                        return db.ProfilePictures.Where(s => s.UserEmail == email).FirstOrDefault().ProfilePicture;
                    }
                }
            }
            return null;
        }

        //Add Activity Log
        public static bool AddActivityLog(string email, string activity_type, string activity_description, string link, string action_id)
        {
            bool process_status = false;
            using (var db = new DBConnection())
            {
                ActivityLogModel activity = new ActivityLogModel
                {
                    Email = email,
                    ActivityType = activity_type,
                    ActivityDescription = activity_description,
                    Link = link,
                    ActionID = action_id,
                    ActivityDate = DateTime.Now
                };
                
                db.ActivityLog.Add(activity);
                
                try
                {
                    db.SaveChanges();
                    process_status = true;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    //Log Error
                    LogError(ex, email, "AddActivityLog", null);
                }

            }
            return process_status;
        }


        //Log Error To File
        public static bool LogError(Exception ex, string user_email, string function_name, string link)
        {
            bool process_status = false;
            string folder_directory = HttpContext.Current.Server.MapPath("~/App_Data/Error/Error.txt");
            //string filePath = @"C:\Error.txt";

            if(ex != null)
            {
                //Log error to db 
                LogErrorToDb(user_email, function_name, link, ex.ToString());
            }

            using (StreamWriter writer = new StreamWriter(folder_directory, true))
            {
                writer.WriteLine("-----------------------------------------------------------------------------");
                writer.WriteLine("Date : " + DateTime.Now.ToString());
                if (!string.IsNullOrEmpty(function_name))
                {
                    writer.WriteLine("Function : " + function_name);
                }
                if (!string.IsNullOrEmpty(user_email))
                {
                    writer.WriteLine("User : " + user_email);
                }
                if (!string.IsNullOrEmpty(link))
                {
                    writer.WriteLine("Link : " + link);
                }

                writer.WriteLine();

                while (ex != null)
                {
                    writer.WriteLine(ex.GetType().FullName);
                    writer.WriteLine("Message : " + ex.Message);
                    writer.WriteLine("StackTrace : " + ex.StackTrace);
                    ex = ex.InnerException;
                    process_status = true;
                }


            }
            return process_status;
        }


        //Log Error To DB
        public static bool LogErrorToDb(string user, string function, string link, string exception)
        {
            bool process_status = false;
            using (var db = new DBConnection())
            {
                ErrorLogModel error = new ErrorLogModel
                {
                    User = user,
                    Function = function,
                    Link = link,
                    Exception = exception,
                    Status = 0,
                    LogDate = DateTime.Now
                };
                
                db.ErrorLog.Add(error);
                
                try
                {
                    db.SaveChanges();
                    process_status = true;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    //Log Error
                    LogError(ex, user, "LogErrorToDb", null);
                }

            }
            return process_status;
        }

        //Generate Random Salt length
        public static int GetRandomSaltLength() {
            int[] numbers = new[] {5, 6, 7, 8};
            Random rnd = new Random();
            int rand_num = numbers[rnd.Next(0, numbers.Length)];
            return rand_num; 
        }

        //Validate email function
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static string ShuffleString(string str)
        {
            char[] array = str.ToCharArray();
            Random rng = new Random();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
            return new string(array);
        }

    }
}