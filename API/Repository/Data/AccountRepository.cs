using API.Context;
using API.Models;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using API.ViewModel;

namespace API.Repository.Data
{
    public class AccountRepository : GeneralRepository<MyContext, Account, string>
    {
        private readonly MyContext myContext;

        public AccountRepository(MyContext myContext): base(myContext)
        {
            this.myContext = myContext;  
        } 

        public int Login (string inputEmail, string inputPassword)
        {
            try
            {
                // var emailRegistered = myContext.Employees.Where(e => e.Email == inputEmail).SingleOrDefault();


                var emailRegistered = (from emp in myContext.Employees
                                          join ac in myContext.Accounts on emp.NIK equals ac.NIK
                                          where emp.Email == inputEmail
                                          select emp.Email).Single();

                var passwordRegistered = (from emp in myContext.Employees
                                          join ac in myContext.Accounts on emp.NIK equals ac.NIK
                                          where emp.Email == inputEmail
                                          select ac.Password).Single();

                var verifyPassword = BCrypt.Net.BCrypt.Verify(inputPassword, passwordRegistered);

                if(inputEmail != emailRegistered && verifyPassword == false)
                {
                    return 1; // account not registered
                }
                else
                {
                    if (inputEmail == emailRegistered)
                    {
                        if (verifyPassword == true)
                        {
                            return 2; // login success
                        }
                        else if (verifyPassword == false)
                        {
                            return 3; // wrong password
                        }
                    }
                }
               
            } catch (Exception)
            {
                return 4; // wrong email
            }

          return 5;
        }

        public IEnumerable GetProfile(string email)
        {
            var profile = (from emp in myContext.Employees
                           join acc in myContext.Accounts on emp.NIK equals acc.NIK 
                           join pro in myContext.Profilings on acc.NIK equals pro.NIK
                           join edu in myContext.Educations on pro.Id equals edu.Id
                           join univ in myContext.Universities on edu.University_Id equals univ.Id
                           where emp.Email == email
                           select new
                           {
                               NIK = emp.NIK,
                               FullName = emp.FirstName + " " + emp.LastName,
                               Phone = emp.Phone,
                               Birthdate = emp.BirthDate,
                               Salary = emp.Salary,
                               Email = emp.Email,
                               Degree = edu.Degree,
                               GPA = edu.GPA,
                               UnivName = univ.Name
                           }).ToList();

            return profile;
        }

        public int ForgotPassword(string email)
        {
            //var emailRegistered = myContext.Employees.Where(e => e.Email == email).SingleOrDefault();
            var sendingEmail = SendEmail(email);    
            if(sendingEmail == true)
            {
                return 2;
            }
            else
            {
                return 3;
            }
          
        }

        public bool SendEmail(string email)
        {
            MimeMessage message = new MimeMessage();
            SmtpClient client = new SmtpClient();
            Random random = new Random();
            string otpRandomNumber = random.Next(0, 1000000).ToString("D6");

            try
            {

                message.From.Add(new MailboxAddress("Demo Project", "hansdemoproject@gmail.com"));
                message.To.Add(MailboxAddress.Parse(email));
                message.Subject = "Reset Password";
                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = $"<h2>Hello Buddy</h2>\nYour OTP number is {otpRandomNumber}"
                };

                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate("hansdemoproject@gmail.com", "HANSportfolio55");
                client.Send(message);
               /* client.Disconnect(true);
                client.Dispose();*/

                return true;    
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
             
                client.Disconnect(true);
                client.Dispose();
            }
        }

    }
}
