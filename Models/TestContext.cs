using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.IO;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Models
{
   public class TestContext
   {

   public string ConnectionString{get;set;}
   public string[] Host{get;set;}

   public TestContext(string connectionString,string[] hosts)
   {
      this.ConnectionString = connectionString;
      this.Host = hosts;
   }
   private MySqlConnection GetConnection()
   {
     return new MySqlConnection(ConnectionString);
   }
public int ForgetPassword(User user)
   {
     int count = 0;
     string password = "";
     try
     {
       using(MySqlConnection conn = GetConnection())
       {
          conn.Open();
          string cmdText = "select password from user where email=@email";
          MySqlCommand cmd = new MySqlCommand(cmdText,conn);
          cmd.Parameters.AddWithValue("email",user.email);
          password = Convert.ToString(cmd.ExecuteScalar());
          conn.Close();
          if(password != "")
           {
               sendPassword(user.email,password);
               count = 1;
           }
          return  count; 
       }
     }
       catch(Exception ex)
       {
         return 0;
       }
     
}
public void sendPassword(string email,string password)
{
   string html =
   @"Password  for your IotAutomatedCloud account
   This email was sent as a remainder for password: " + password + " " ;

    var fromAddress = new MailAddress("iotcloudautomated@gmail.com", "iotcloudautomated");
    var toAddress = new MailAddress(email, "PasswordForget@IotAutomatedCloud.com");
    const string fromPassword = "105481Do";
    const string subject = "Password for your IotAutomatedCloud account";
    string body = html;

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
        smtp.Send(message);
    }
}

public int PutUser(User user)
{
     int count = 0;
     string flag = "T";

       using(MySqlConnection conn = GetConnection())
       {
          conn.Open();
          string cmdText = "update user set flag = @flag where email =@email";
          MySqlCommand cmd = new MySqlCommand(cmdText,conn);
          cmd.Parameters.AddWithValue("email",user.email);
          cmd.Parameters.AddWithValue("flag",flag);
          count = cmd.ExecuteNonQuery();
          conn.Close();
          return count;
       }
}

public String PostUser(User user)
{
     int count = 0;
     string flag = "T";
     try{
       using(MySqlConnection conn = GetConnection())
       {
          conn.Open();
          string cmdText = "select * from  user where email = @email and password=@password and flag = @flag";
          MySqlCommand cmd = new MySqlCommand(cmdText,conn);
          cmd.Parameters.AddWithValue("email",user.email);
          cmd.Parameters.AddWithValue("password",user.password);
          cmd.Parameters.AddWithValue("flag",flag);
          count = Convert.ToInt32(cmd.ExecuteScalar());
          conn.Close();
       }
       return count.ToString();
     }
     catch(Exception ex)
     {
       return ex.ToString();
     }
}

public void sendMail(string email)
{
  string html =
   @"Actication link for your IotAutomatedCloud account
   This email was sent activation
   https://localhost:5000/api/activateUser/"+email+">Activation Link</a> click it.";

    var fromAddress = new MailAddress("iotcloudautomated@gmail.com", "iotcloudautomated");
    var toAddress = new MailAddress(email, "Activation@IotAutomatedCloud.com");
    const string fromPassword = "105481Do";
    const string subject = "Activation link for IotAutomatedCloud account";
    string body = html;

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
        smtp.Send(message);
    }
}


public int PostUpUser(User user)
{
    int count = 0;
    string flag = "F";

    using(MySqlConnection conn = GetConnection())
    {
        conn.Open();
        string cmdText = "select * from  user where email = @email";
        MySqlCommand cmd = new MySqlCommand(cmdText,conn);
        cmd.Parameters.AddWithValue("email",user.email);
        count = Convert.ToInt32(cmd.ExecuteScalar());
        conn.Close();
    }

    if(count == 0)
    {
       using(MySqlConnection conn = GetConnection())
       {
          conn.Open();
          string cmdText = "insert into user(name,password,email,flag) values(@name,@password,@email,@flag)";
          MySqlCommand cmd = new MySqlCommand(cmdText,conn);
          cmd.Parameters.AddWithValue("name",user.name);
          cmd.Parameters.AddWithValue("password",user.password);
          cmd.Parameters.AddWithValue("email",user.email);
          cmd.Parameters.AddWithValue("flag",flag);
          int rows = cmd.ExecuteNonQuery();
          if(rows>0)
             sendMail(user.email);
          conn.Close();
          return rows;
       }
    }

    else
    {
        return 0;
    }

}
   }}