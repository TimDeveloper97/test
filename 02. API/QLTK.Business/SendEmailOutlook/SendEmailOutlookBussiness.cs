using NTS.Model.Notify;
using QLTK.Business.RabbitMQServers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace QLTK.Business.SendEmailOutlook
{
    public class SendEmailOutlookBussiness
    {
        private readonly RabbitMQServer<MessageNotify> rabbitMQServer = new RabbitMQServer<MessageNotify>();

        // Hotmail, Outlook.com SMTP Server: smtp.live.com
        // Office 365 SMTP Server : smtp.office365.com
        // SMTP Gmail: smtp.gmail.com
        // SMTP Outlook: smtp-mail.outlook.com

        public bool SendMail(string emailSend, string passSend, string emailInbox, string title, string content)
        {
            emailSend = "chinhvip959@gmail.com";
            passSend = "09011997";
            emailInbox = "chinhl229@gmail.com";
            title = "SendMail";
            content = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            try
            {
                MailMessage mailsend = new MailMessage();
                mailsend.To.Add(emailInbox);
                mailsend.From = new MailAddress(emailSend);
                mailsend.Subject = title;
                mailsend.Body = content;
                mailsend.IsBodyHtml = true;
                int cong = 587;
                SmtpClient client = new SmtpClient("smtp.office365.com")
                {
                    Port = cong,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,

                };
                NetworkCredential credentials = new NetworkCredential(emailSend, passSend);
                client.Credentials = credentials;
                client.Send(mailsend);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void SendEMailThroughOUTLOOK()
        {
            try
            {
                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();
                // Create a new mail item.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                // Set HTMLBody. 
                //add the body of the email
                oMsg.HTMLBody = "Hello, Jawed your message body will go here!!";
                //Add an attachment.
                String sDisplayName = "MyAttachment";
                int iPosition = (int)oMsg.Body.Length + 1;
                int iAttachType = (int)Outlook.OlAttachmentType.olByValue;
                //now attached the file
                Outlook.Attachment oAttach = oMsg.Attachments.Add
                                             (@"C:\Users\chinh\Downloads\bc17ff26422cb672ef3d.jpg", iAttachType, iPosition, sDisplayName);
                //Subject line
                oMsg.Subject = "Your Subject will go here.";
                // Add a recipient.
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                // Change the recipient in the next line if necessary.
                oRecips.Add("chinhvip959@gmail.com");
                Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add("duy.lp@nhantinsoft.vn");
                oRecip.Resolve();
                // Send.
                oMsg.Send();
                // Clean up.
                oRecip = null;
                oRecips = null;
                oMsg = null;
                oApp = null;
            }//end of try block
            catch (Exception ex)
            {
            }//end of catch
        }//end of Email 

        #region
        //public static bool sendEmailViaOutlook(string sFromAddress, string sToAddress, string sCc, string sSubject, string sBody, BodyType bodyType, List<string> arrAttachments = null, string sBcc = null)
        //{
        //    //Send email via Office Outlook 2010
        //    //'sFromAddress' = email address sending from (ex: "me@somewhere.com") -- this account must exist in Outlook. Only one email address is allowed!
        //    //'sToAddress' = email address sending to. Can be multiple. In that case separate with semicolons or commas. (ex: "recipient@gmail.com", or "recipient1@gmail.com; recipient2@gmail.com")
        //    //'sCc' = email address sending to as Carbon Copy option. Can be multiple. In that case separate with semicolons or commas. (ex: "recipient@gmail.com", or "recipient1@gmail.com; recipient2@gmail.com")
        //    //'sSubject' = email subject as plain text
        //    //'sBody' = email body. Type of data depends on 'bodyType'
        //    //'bodyType' = type of text in 'sBody': plain text, HTML or RTF
        //    //'arrAttachments' = if not null, must be a list of absolute file paths to attach to the email
        //    //'sBcc' = single email address to use as a Blind Carbon Copy, or null not to use
        //    //RETURN:
        //    //      = true if success
        //    bool bRes = false;

        //    try
        //    {
        //        //Get Outlook COM objects
        //        Outlook.Application app = new Outlook.Application();
        //        Outlook.MailItem newMail = (Outlook.MailItem)app.CreateItem(Outlook.OlItemType.olMailItem);

        //        //Parse 'sToAddress'
        //        if (!string.IsNullOrWhiteSpace(sToAddress))
        //        {
        //            string[] arrAddTos = sToAddress.Split(new char[] { ';', ',' });
        //            foreach (string strAddr in arrAddTos)
        //            {
        //                if (!string.IsNullOrWhiteSpace(strAddr) &&
        //                    strAddr.IndexOf('@') != -1)
        //                {
        //                    newMail.Recipients.Add(strAddr.Trim());
        //                }
        //                else
        //                    throw new Exception("Bad to-address: " + sToAddress);
        //            }
        //        }
        //        else
        //            throw new Exception("Must specify to-address");

        //        //Parse 'sCc'
        //        if (!string.IsNullOrWhiteSpace(sCc))
        //        {
        //            string[] arrAddTos = sCc.Split(new char[] { ';', ',' });
        //            foreach (string strAddr in arrAddTos)
        //            {
        //                if (!string.IsNullOrWhiteSpace(strAddr) &&
        //                    strAddr.IndexOf('@') != -1)
        //                {
        //                    newMail.Recipients.Add(strAddr.Trim());
        //                }
        //                else
        //                    throw new Exception("Bad CC-address: " + sCc);
        //            }
        //        }

        //        //Is BCC empty?
        //        if (!string.IsNullOrWhiteSpace(sBcc))
        //        {
        //            newMail.BCC = sBcc.Trim();
        //        }

        //        //Resolve all recepients
        //        if (!newMail.Recipients.ResolveAll())
        //        {
        //            throw new Exception("Failed to resolve all recipients: " + sToAddress + ";" + sCc);
        //        }


        //        //Set type of message
        //        switch (bodyType)
        //        {
        //            case BodyType.HTML:
        //                newMail.HTMLBody = sBody;
        //                break;
        //            case BodyType.RTF:
        //                newMail.RTFBody = sBody;
        //                break;
        //            case BodyType.PlainText:
        //                newMail.Body = sBody;
        //                break;
        //            default:
        //                throw new Exception("Bad email body type: " + bodyType);
        //        }


        //        if (arrAttachments != null)
        //        {
        //            //Add attachments
        //            foreach (string strPath in arrAttachments)
        //            {
        //                if (File.Exists(strPath))
        //                {
        //                    newMail.Attachments.Add(strPath);
        //                }
        //                else
        //                    throw new Exception("Attachment file is not found: \"" + strPath + "\"");
        //            }
        //        }

        //        //Add subject
        //        if (!string.IsNullOrWhiteSpace(sSubject))
        //            newMail.Subject = sSubject;

        //        Outlook.Accounts accounts = app.Session.Accounts;
        //        Outlook.Account acc = null;

        //        //Look for our account in the Outlook
        //        foreach (Outlook.Account account in accounts)
        //        {
        //            if (account.SmtpAddress.Equals(sFromAddress, StringComparison.CurrentCultureIgnoreCase))
        //            {
        //                //Use it
        //                acc = account;
        //                break;
        //            }
        //        }

        //        //Did we get the account
        //        if (acc != null)
        //        {
        //            //Use this account to send the e-mail. 
        //            newMail.SendUsingAccount = acc;

        //            //And send it
        //            ((Outlook._MailItem)newMail).Send();

        //            //Done
        //            bRes = true;
        //        }
        //        else
        //        {
        //            throw new Exception("Account does not exist in Outlook: " + sFromAddress);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("ERROR: Failed to send mail: " + ex.Message);
        //    }

        //    return bRes;
        //}
        #endregion

        public void btnSend_Click()
        {
            #region Mở outlook
            int count = Process.GetProcesses().Where(o => o.ProcessName.Contains("OUTLOOK")).Count();
            if (count == 0)
            {
                try
                {
                    Process.Start("outlook.exe");
                }
                catch (Exception)
                {
                }
            }
            #endregion Mở outlook

            List<string> at = new List<string>();

            string txtContent = "<br>"
            + "<table style=\"width:100%\" border=\"1\">"
             + " <tr>"
               + " <td style=\"width:20%\"><b>Mô tả chi tiết</b></td>"
                + "</td> "
             + "</tr>"
            + "</table>";

            bool success = SetEmailSendHasAttach("Thử", txtContent, "chinhvip959@gmail.com", "", at);
            if (success)
            {
                //MessageBox.Show("Đã gửi mail thành công!", TextUtils.Caption);
                //Description = txtDetail.Text.Trim();
                //this.DialogResult = DialogResult.OK;
                //if (IsClosed)
                //{
                //    this.Close();
                //}
            }
            else
            {
                //MessageBox.Show("Gửi không thành công!", TextUtils.Caption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        public static bool SetEmailSendHasAttach(string sSubject, string sBody, string sTo, string cc, List<string> attachments)
        {
            bool isSuccess;
            try
            {
                Outlook.Application oApp = new Outlook.Application();
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                Outlook.Inspector oInspector = oMsg.GetInspector;
                oMsg.Subject = sSubject;
                oMsg.To = sTo;
                oMsg.CC = cc;

                oMsg.Display();
                oMsg.HTMLBody = sBody + oMsg.HTMLBody;

                if (attachments.Count > 0)
                {
                    foreach (string attachment in attachments)
                    {
                        if (File.Exists(attachment))
                            oMsg.Attachments.Add(attachment);
                    }
                }

                //oMsg.Send();
                isSuccess = true;

                oMsg = null;
                oApp = null;
            }
            catch (Exception)
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        public void PushQueue()
        {
            MessageNotify messageNotify = new MessageNotify();
            NotifyEmailOutlookModel notifyEmailModel = new NotifyEmailOutlookModel
            {
                AutoSend = false,
                SubjectTitle = "Gửi email Outlook",
                EmailTo = "chinhl229@gmail.com",
                BodyContent = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                Attachments = new List<string>()
            };

            messageNotify.SentDate = DateTime.Now;
            messageNotify.Type = NotifyType.EmailOutlook;
            messageNotify.MessageContent = notifyEmailModel;

            rabbitMQServer.PushToQueue(messageNotify);
        }
    }
}
