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
using Microsoft.EntityFrameworkCore;

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
                //var emailRegistered = myContext.Employees.Where(e => e.Email == inputEmail).SingleOrDefault();
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
                           join roleAcc in myContext.RoleAccounts on emp.NIK equals roleAcc.NIK
                           join role in myContext.Roles on roleAcc.Id equals role.Id
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
                               UnivName = univ.Name,
                               RoleName = role.Name
                           }).ToList();
            return profile;
        }

        public int ForgotPassword(string email)
        {
            var emailRegistered = myContext.Employees.Where(e => e.Email == email).SingleOrDefault();

            if(emailRegistered != null)
            {
                var sendingEmail = SendEmail(email, emailRegistered);
                if (sendingEmail == true)
                {
                    return 2;
                }
                else
                {
                    return 3;
                }
            }
            else
            {
                return 4;
            }
        }

        public bool SendEmail(string email, Employee employeeNIK)
        {
            MimeMessage message = new MimeMessage();
            SmtpClient client = new SmtpClient();
            Random random = new Random();

            var checkAccount = myContext.Accounts.Where(a => a.NIK == employeeNIK.NIK).SingleOrDefault();
            string otpRandomNumber = random.Next(0, 1000000).ToString("D6");

            try
            {
                var timeExpiry = DateTime.Now.AddMinutes(5);
                checkAccount.OTP = Convert.ToInt32(otpRandomNumber);
                checkAccount.ExpiredToken = timeExpiry;
                checkAccount.isUsed = false;

                myContext.Entry(checkAccount).State = EntityState.Modified;
                myContext.SaveChanges();

                message.From.Add(new MailboxAddress($"Reset Password {DateTime.Now.ToString("dddd, dd MMMM yyyy")}", "hansdemoproject@gmail.com"));
                message.To.Add(MailboxAddress.Parse(email));
                message.Subject = "Reset Password";
                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = $"<h2>Hello Buddy</h2> Your OTP number is <strong>{otpRandomNumber}</strong> </br></br> This OTP will expire on 5 minutes"
                };

                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate("hansdemoproject@gmail.com", "HANSportfolio55");
                client.Send(message);
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

        public int ChangePassword(string email, int otp, string newPassword, string confirmNewPassword)
        {
           //var emailCheck = myContext.Employees.Where(e => e.Email == email).SingleOrDefault();
           //var otpCheck2 = myContext.Accounts.Where(a => a.OTP == otp).SingleOrDefault();

            var acc = new Account();
            var otpCheck = myContext.Accounts.Where(a => a.OTP == otp).Select(a => a.OTP).SingleOrDefault();
            var otpExpiryCheck = IsOTPExpired(acc.ExpiredToken);
            var accountCheck = myContext.Accounts.Where(a => a.OTP == otp).SingleOrDefault();
            var confirmPasswordCheck = IsConfirmPassword(newPassword, confirmNewPassword);

            var otpUsedCheck = IsOTPUsed(otpCheck);

            if (otpCheck == otp)
            {
                if(otpExpiryCheck == false)
                {
                    if(confirmPasswordCheck == false)
                    {
                        if(otpUsedCheck == false)
                        {
                            accountCheck.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                            accountCheck.isUsed = true;

                            myContext.Entry(accountCheck).State = EntityState.Modified;
                            myContext.SaveChanges();
                            return 5; // password has been changed
                        }
                        else
                        {
                            return 6; // is used
                        }
                    }
                    else
                    {
                        return 7; // wrong input confirm new password
                    }
                }
                else
                {
                    return 8; // otp expired
                }
            }
            else
            {
                return 9; // otp is wrong
            }
        }

        public bool IsOTPExpired(DateTime expiryDate)
        {
            if (expiryDate < DateTime.Now)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsConfirmPassword(string newPassword, string confirmPassword)
        {

            if(newPassword != confirmPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsOTPUsed(int OTP)
        {
            var otpUsedCheck = myContext.Accounts.Where(a => a.OTP == OTP).Select(a => a.isUsed).SingleOrDefault();

            if (otpUsedCheck != false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
