using NoteIsMe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace NoteIsMe.UWP.ViewModels
{
    public class UserViewModel : BindableBase
    {
        public User CurrentUser { get; set; }


        bool _IsModified = false;

        public bool IsModified
        {
            get => _IsModified;
            set
            {
                if (value != _IsModified)
                {

                    _IsModified = value;
                    OnPropertyChanged();

                }
            }
        }

        private User _user;

        public User user
        {
            get => _user;
            set
            {
                Set(ref _user, value);
                OnPropertyChanged();
            }
        }


        public ObservableCollection<User> Users { get; set; }

        public UserViewModel()
        {
            CurrentUser = new User();
            user = new User();
            Users = new ObservableCollection<User>();
        }

        public Uri getUserGravatarURI(string email)
        {
            string imageHash = App.userViewModel.HashEmailForGravatar(email);
            string ImagePath = $"http://www.gravatar.com/avatar/{imageHash}";
            Uri resourceUri = new Uri(@ImagePath, UriKind.RelativeOrAbsolute);

            return resourceUri;
        }

        public async Task LoadAllAsync()
        {

            List<User> list = await App.UnitOfWork.UserRepository.FindAllAsync();

            Users.Clear();
            foreach (User e in list)
            {
                e.Notes = await App.UnitOfWork.NoteRepository.FindByUserIdAsync(e.Id);
                e.Sketches = await App.UnitOfWork.SketchRepository.FindByUserIdAsync(e.Id);
                e.Notebooks = await App.UnitOfWork.NotebookRepository.FindByUserIdAsync(e.Id);
                e.Folders = await App.UnitOfWork.FolderRepository.FindByUserIdAsync(e.Id);
                e.Groups = await App.UnitOfWork.GroupRepository.FindByUserIdAsync(e.Id);
                
                Users.Add(e);
            }
        }

        internal async Task DeleteAsync(User user)
        {
            user.Notes = await App.UnitOfWork.NoteRepository.FindByUserIdAsync(user.Id);
            user.Sketches = await App.UnitOfWork.SketchRepository.FindByUserIdAsync(user.Id);
            user.Notebooks = await App.UnitOfWork.NotebookRepository.FindByUserIdAsync(user.Id);
            user.Folders = await App.UnitOfWork.FolderRepository.FindByUserIdAsync(user.Id);
            user.Groups = await App.UnitOfWork.GroupRepository.FindByUserIdAsync(user.Id);
            user.Tags = await App.UnitOfWork.TagRepository.FindAllMyTagsAsync(user.Id);

            await App.UnitOfWork.UserRepository.DeleteUserAsync(user);

        }

        //user test code



        public bool IsAuthenticated()
        {
            // get user auth value from local settings
            object authObject = ApplicationData.Current.LocalSettings.Values["isAuthenticated"];




            if (authObject != null)
            {
                return (Boolean)authObject;
            }
            else
            {
                return false;
            }
        }

        public async Task LogIn(int id)
        {
            Task<User> usertask = App.UnitOfWork.UserRepository.FindByIdAsync(id);
            App.userViewModel.CurrentUser = await usertask;
            ApplicationData.Current.LocalSettings.Values["currentUserID"] = id;
            ApplicationData.Current.LocalSettings.Values["isAuthenticated"] = true;
        }

        public void LogOut()
        {
            ApplicationData.Current.LocalSettings.Values["isAuthenticated"] = false;
            ApplicationData.Current.LocalSettings.Values["currentUserID"] = 0;

        }

        public int GetCurrentUserID()
        {
            object curID = ApplicationData.Current.LocalSettings.Values["currentUserID"];
            int currentUserID = (int)curID;

            return currentUserID;
        }

        public string CreateSalt()
        {
            int size = 32;
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }


        public string GenerateHash(string input, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input + salt);
            SHA256Managed sHA256ManagedString = new SHA256Managed();
            byte[] hash = sHA256ManagedString.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public bool VerifyPassword(string plainTextInput, string hashedInput, string salt)
        {
            string newHashedPin = GenerateHash(plainTextInput, salt);
            return newHashedPin.Equals(hashedInput);
        }




        public async void sendEmail(int code, string email, string name, string type)
        {

            var fromAddress = new MailAddress("noreply.noteisme@gmail.com", "Noreply by NoteIsMe");
            var toAddress = new MailAddress(email, name);
            const string fromPassword = "AD2021NIM";
            string subject = "Note Is Me Code";
            string body = "Note is Me thanks you";


            if (type == "reset")
            {
                subject = "Reset Password - NoteIsMe";
                body = $"Hello, We have received a request to reset your password on NoteIsMe, please use the code <br> <br> <b><h3> {code.ToString()} </h3></b> <br> <br> to reset the password. <br> <br> If your didn't request this code please kindly ignore. <br> <br> <hr> Thank you, <br> NoteIsMe";
            }
            else if (type == "register")
            {
                subject = "Verify Account - NoteIsMe";
                body = $"Hello, Thank You for registering  <br> <br> <b><h3> {code.ToString()} </h3></b> <br> <br> to verify your account. <br> <br> If your didn't register please kindly ignore. <br> <br> <hr> Thank you, <br> NoteIsMe";

            }

            else if (type == "share")
            {
                Notebook notebook = await App.UnitOfWork.NotebookRepository.FindByIdAsync(code);
                subject = "Notebook shared";
                body = $"Hello, <br>A new notebook ({notebook.Title}) has been shared to you by {App.userViewModel.CurrentUser.Name}. Please open NoteIsMe application to browse/edit notes and sketches inside the notebook. <br> <br> Please kindly report to us if you didn't expect this email. <br> <br> <hr> Thank you, <br> NoteIsMe";

            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {

                Subject = subject,
                Body = body
            })
            {
                message.IsBodyHtml = true;
                smtp.Send(message);
            }
        }


        /// Hashes an email with MD5.  Suitable for use with Gravatar profile
        /// image urls
        public string HashEmailForGravatar(string email)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.  
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.  
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(email));

            // Create a new Stringbuilder to collect the bytes  
            // and create a string.  
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string.  
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();  // Return the hexadecimal string. 
        }

        public static string getUserName(int id)
        {
            User user = App.UnitOfWork.UserRepository.FindById(id);
            return user.Name;
        }
    }
}
