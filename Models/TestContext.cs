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
   @"<html>
   <head></head>
   <body>
   <h1>Password  for your IotAutomatedCloud account</h1>
   <p>This email was sent as a remainder for password: "+ password +  "</p></body></html>";

   SmtpClient client =   new SmtpClient(Host[0],Convert.ToInt32(Host[1]));
   System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();

   msg.From = new System.Net.Mail.MailAddress("PasswordForget@IotAutomatedCloud.com");
   msg.Subject = "Password for your IotAutomatedCloud account";
   List<String> mailgrup = new List<String>();
   mailgrup.Add(email);

   foreach(var list in mailgrup)
    msg.To.Add(list);
   
   AlternateView htmlView = AlternateView.CreateAlternateViewFromString
   (
     html,
     Encoding.UTF8,
     MediaTypeNames.Text.Html
   );

   msg.AlternateViews.Add(htmlView);
   msg.IsBodyHtml = true;
   client.Send(msg);
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

public int PostUser(User user)
{
     int count = 0;
     string flag = "T";

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
       return count;
}

public void sendMail(string email)
{
  string html =
   @"<html>
   <head></head>
   <body>
   <h1>Actication link for your IotAutomatedCloud account</h1>
   <p>This email was sent activation
   <a href=https://localhost:5000/api/activateUser/"+email+">Activation Link</a> click it.</p></body></html>";

   SmtpClient client =   new SmtpClient(Host[0],Convert.ToInt32(Host[1]));
   System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();

   msg.From = new System.Net.Mail.MailAddress("Activation@IotAutomatedCloud.com");
   msg.Subject = "Activation link for IotAutomatedCloud account";
   List<String> mailgrup = new List<String>();
   mailgrup.Add(email);

   foreach(var list in mailgrup)
    msg.To.Add(list);
   
   AlternateView htmlView = AlternateView.CreateAlternateViewFromString
   (
     html,
     Encoding.UTF8,
     MediaTypeNames.Text.Html
   );

   msg.AlternateViews.Add(htmlView);
   msg.IsBodyHtml = true;
   client.Send(msg);

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