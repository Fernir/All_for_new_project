using Alex.WoWRelogger;
using Alex.WoWRelogger.Settings;
using HighVoltz.HBRelog.Source.DB;
using HighVoltz.HBRelog.Source.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Resources;
using xNet;

namespace Alex.WoWRelogger.Utility
{
	internal static class AccountCreator
	{
		private const string DEFAULT_PASS = "absolute123";

        /// <summary>
        /// генератор случайной последовательности
        /// </summary>
		private static Random random;

        /// <summary>
        /// список фамилий
        /// </summary>
		private static string[] surnames;

        /// <summary>
        /// список имен
        /// </summary>
		private static string[] names;

        /// <summary>
        /// страница оплаты веб мини
        /// </summary>
		private static string WebmoneyPage;

        /// <summary>
        /// Инициализация списков
        /// </summary>
		static AccountCreator()
		{
			random = new Random();

		    using (
		        var streamReader =
		            new StreamReader(Application.GetResourceStream(new Uri("/Data/webmoney.html", UriKind.Relative)).Stream,
		                Encoding.UTF8))
		    {
		        WebmoneyPage = streamReader.ReadToEnd();
		    }

		    using (
		        var streamReader =
		            new StreamReader(Application.GetResourceStream(new Uri("/Data/surnames.txt", UriKind.Relative)).Stream,
		                Encoding.UTF8))
		    {
		        var strs = new List<string>();
		        while (true)
		        {
		            var str = streamReader.ReadLine();
		            if (str == null)
		                break;
		            strs.Add(str);
		        }
		        surnames = strs.ToArray();
		    }

		    using (
		        var streamReader =
		            new StreamReader(Application.GetResourceStream(new Uri("/Data/names.txt", UriKind.Relative)).Stream,
		                Encoding.UTF8))
		    {
		        var strs = new List<string>();
		        while (true)
		        {
		            var str = streamReader.ReadLine();
		            if (str == null)
		                break;

		            strs.Add(str);
		        }
		        names = strs.ToArray();
		    }
		}

        /// <summary>
        /// Создание и оплата акаунта
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="accountName"></param>
        /// <param name="answer"></param>
	    public static void CreateAndPay(string email = null, string password = null, string accountName = null,
	        string answer = null)
	    {
	        lock ("CreateAndPayBlock")
	        {
	            var siteAsString = "";
	            var accountTool = new AccountTools();
	            if (email == null)
	            {
	                email = GenerateEmail();
	            }
	            if (password == null)
	            {
                    password = DEFAULT_PASS;
	            }
	            if (answer == null)
	            {
	                answer = GenerateRandomString();
	            }
                var firstname = names[random.Next(0, names.Length - 1)]; 
                var lastname = surnames[random.Next(0, surnames.Length - 1)];
                
                accountTool.ClearCookies();
                accountTool.ChangeProxy();

	            var proxy = new Account
	            {
	                AccountName = "WoW1",
	                Answer = answer,
	                Email = email,
	                Password = password,
	                CreationDate = DateTime.Now,
	                Status = "TRIAL",
	                RegIp = accountTool.GetProxy()
	            };

	            try
	            {
	                siteAsString = accountTool.GetSiteAsString("https://eu.battle.net/account/creation/tos.html", false,
	                    null, true);
	            }
	            catch (Exception exception)
	            {
	                siteAsString = accountTool.GetSiteAsString("https://eu.battle.net/account/creation/tos.html", false,
	                    null, true);
	            }
	            var item = Regex.Match(siteAsString, "name=\"csrftoken\" value=\"(.{36})\"").Groups[1]; // получить токен
	            if (!Regex.Match(siteAsString, "\"/account/captcha\\.jpg\\?(.+?)\"").Success) // если нет капчи то выполняем регистрацию
	            {
                    //регистрируемся
	                siteAsString = accountTool.PostSite("https://eu.battle.net/account/ru/creation/tos.html",
	                    string.Format("csrftoken={0}&country=RUS&firstname={1}&lastname={2}&emailAddress={3}&password={4}&rePassword={4}&question1=20&answer1={6}&age=22&agreedToPrivacyPolicy=on{5}",
                            item, firstname, lastname, email, password, "", answer), false, null, true);
	                item = Regex.Match(siteAsString, "name=\"csrftoken\" value=\"(.{36})\"").Groups[1];
	                if (siteAsString.Contains("Код безопасности введен неверно. Попробуйте еще раз."))
	                {
	                    MessageBox.Show("Wrong captcha code!");
	                }
	                else if (siteAsString.Contains("Этот E-mail уже использован."))
	                {
	                    MessageBox.Show("Этот E-mail уже использован.");
	                }
	                else if (siteAsString.Contains("Создание записи завершено"))
	                {
	                    siteAsString =
	                        accountTool.GetSiteAsString("https://eu.battle.net/account/management/add-game-trial.html?type=WOWT&gameRegion=EU&locale=en_US",
	                            false, null, true);
	                    item = Regex.Match(siteAsString, "name=\"csrftoken\" value=\"(.{36})\"").Groups[1];
	                    
                        siteAsString = accountTool.PostSite("https://eu.battle.net/account/management/add-game-trial.html",
	                        string.Format("csrftoken={0}&type=WOWT&gameRegion=EU&confirmed=", item), false, null, true);
	                    
                        //открываем страницу оплаты с промокодом
                        siteAsString = accountTool.GetSiteAsString(
	                            "https://eu.battle.net/shop/ru-RU/checkout/promotion/EU/2210043971000006457?accountName&app=shop&cr=true",
	                            false, null, true);
                        //получаем уникальный код для оплаты
	                    var group = Regex.Match(siteAsString, "https://eu.battle.net/shop/en/checkout/pay/(.+?)\"").Groups[1];
                        
                        siteAsString =
                            accountTool.GetSiteAsString(
                               string.Concat("https://eu.battle.net/shop/ru/checkout/webmoney/", group), true, null, false);

                        var itemVM = Regex.Match(siteAsString, "name=\"csrftoken\" value=\"(.{36})\"").Groups[1];

                        var st =
                            accountTool.PostSiteRaw(string.Concat("https://eu.battle.net/shop/ru/checkout/webmoney/", group),
                                string.Format("csrftoken={0}&paymentOption=/shop/ru/checkout/webmoney/{1}", itemVM, group),
                                true, string.Concat("https://eu.battle.net/shop/ru/checkout/pay/", group), false);

                        
                        string item1 =
	                        accountTool.GetSiteRaw(
	                            string.Concat("https://eu.battle.net/shop/ru/checkout/webmoney/finish/", group), true,
	                            string.Concat("https://eu.battle.net/shop/ru/checkout/pay/", group), false)[
	                                "X-Shop-Redirect"];

	                    siteAsString = accountTool.GetSiteAsString(item1, false, null, true);
	                    string value =
	                        Regex.Match(siteAsString, "<input type=\"hidden\" name=\"LMI_PAYEE_PURSE\" value=\"(.+?)\">")
	                            .Groups[1].Value;
	                    string value1 =
	                        Regex.Match(siteAsString, "<input type=\"hidden\" name=\"LMI_PAYMENT_AMOUNT\" value=\"(.+?)\">")
	                            .Groups[1].Value;
	                    string value2 =
	                        Regex.Match(siteAsString, "<input type=\"hidden\" name=\"LMI_PAYMENT_DESC\" value=\"(.+?)\">")
	                            .Groups[1].Value;
	                    string value3 =
	                        Regex.Match(siteAsString, "<input type=\"hidden\" name=\"LMI_SHOP_ID\" value=\"(.+?)\">")
	                            .Groups[1].Value;
	                    string str3 =
	                        Regex.Match(siteAsString, "<input type=\"hidden\" name=\"LMI_RESULT_URL\" value=\"(.+?)\">")
	                            .Groups[1].Value;
	                    string value4 =
	                        Regex.Match(siteAsString, "<input type=\"hidden\" name=\"LMI_SUCCESS_METHOD\" value=\"(.+?)\">")
	                            .Groups[1].Value;
	                    string str4 =
	                        Regex.Match(siteAsString, "<input type=\"hidden\" name=\"LMI_FAIL_METHOD\" value=\"(.+?)\">")
	                            .Groups[1].Value;
	                    string value5 =
	                        Regex.Match(siteAsString, "<input type=\"hidden\" name=\"REF\" value=\"(.+?)\">").Groups[1]
	                            .Value;
	                    string str5 =
	                        Regex.Match(siteAsString, "<input type=\"hidden\" name=\"PAYMENTREFERENCE\" value=\"(.+?)\">")
	                            .Groups[1].Value;
	                    string str6 = string.Concat(new object[] {"http://127.0.0.1:", 55556, "/success-", value2});
	                    string str7 = string.Concat("http://127.0.0.1:", 55556, "/fail");
	                    proxy.PaymentInfo = value2;
	                    if (!HbRelogManager.Settings.AutoCreateAccounts)
	                    {
	                        proxy.Status = "TRIAL";
	                        AccountManager.AddAccount(proxy);
	                        HttpServer.Get().SetHandler((string Uri) =>
	                        {
	                            if (Uri == "/start")
	                            {
	                                return
	                                    AccountCreator.WebmoneyPage.Replace("@Model.LMI_PAYEE_PURSE", value)
	                                        .Replace("@Model.LMI_PAYMENT_AMOUNT", value1)
	                                        .Replace("@Model.LMI_PAYMENT_DESC", value2)
	                                        .Replace("@Model.LMI_SHOP_ID", value3)
	                                        .Replace("@Model.LMI_RESULT_URL", str3)
	                                        .Replace("@Model.LMI_SUCCESS_URL", str6)
	                                        .Replace("@Model.LMI_SUCCESS_METHOD", value4)
	                                        .Replace("@Model.LMI_FAIL_URL", str7)
	                                        .Replace("@Model.LMI_FAIL_METHOD", str4)
	                                        .Replace("@Model.REF", value5)
	                                        .Replace("@Model.PAYMENTREFERENCE", str5);
	                            }
	                            if (Uri.StartsWith("/success"))
	                            {
	                                string str = Uri.Substring(9);
	                                if (str != null && str.Length > 0)
	                                {
	                                    Account account = (
	                                        from a in AccountManager.GetAccounts()
	                                        where a.PaymentInfo == str
	                                        select a).FirstOrDefault<Account>();
	                                    if (account == null)
	                                    {
	                                        return "Account not found!";
	                                    }
	                                    return
	                                        string.Format(
	                                            "<html><body><h1>Completed!</h1><br><h6>Login: {0}</h6><br><h6>Password: {1}</h6><br><h6>Account: {2}</h6></body></html>",
	                                            account.Email, account.Password, account.AccountName);
	                                }
	                            }
	                            if (Uri == "/fail")
	                            {
	                                return "Payment Failed";
	                            }
	                            return "Not Found";
	                        });
	                        Process.Start(string.Concat("http://127.0.0.1:", 55556, "/start"));
	                    }
	                    else
	                    {
	                        List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>()
	                        {
	                            new KeyValuePair<string, string>("LMI_PAYEE_PURSE", value),
	                            new KeyValuePair<string, string>("LMI_PAYMENT_AMOUNT", value1),
	                            new KeyValuePair<string, string>("LMI_PAYMENT_DESC", value2),
	                            new KeyValuePair<string, string>("LMI_SHOP_ID", value3),
	                            new KeyValuePair<string, string>("LMI_RESULT_URL", str3),
	                            new KeyValuePair<string, string>("LMI_SUCCESS_METHOD", value4),
	                            new KeyValuePair<string, string>("LMI_FAIL_METHOD", str4),
	                            new KeyValuePair<string, string>("REF", value5),
	                            new KeyValuePair<string, string>("PAYMENTREFERENCE", str5),
	                            new KeyValuePair<string, string>("LMI_SUCCESS_URL", str6),
	                            new KeyValuePair<string, string>("LMI_FAIL_URL", str7)
	                        };
	                        siteAsString = accountTool.PostWebmoney("https://merchant.webmoney.ru:443/lmi/payment.asp",
	                            keyValuePairs);
	                        Group group1 = Regex.Match(siteAsString, "name=\"ticket\" value=\"([0-9A-Z]+)\"").Groups[1];
	                        Group item2 = Regex.Match(siteAsString, "\"(GenImg\\.asp\\?rnd=.+?)\"").Groups[1];
	                        string str8 = string.Concat("https://merchant.webmoney.ru:443/lmi/", item2.Value);
	                        StringBuilder stringBuilder = new StringBuilder();
	                        byte[] webmoneyPicture = accountTool.GetWebmoneyPicture(str8);
	                        stringBuilder.Append(Convert.ToBase64String(webmoneyPicture, 0, (int) webmoneyPicture.Length));
	                        string str9 = CaptchaSolver.Solve(stringBuilder.ToString());
	                        keyValuePairs.Clear();
	                        keyValuePairs.Add(new KeyValuePair<string, string>("s_auth_type", "authtype_17"));
	                        keyValuePairs.Add(new KeyValuePair<string, string>("pinnumber", ""));
	                        keyValuePairs.Add(new KeyValuePair<string, string>("pincode", ""));
	                        keyValuePairs.Add(new KeyValuePair<string, string>("mobilecaptcha", ""));
	                        keyValuePairs.Add(new KeyValuePair<string, string>("lightcaptcha", str9));
	                        keyValuePairs.Add(new KeyValuePair<string, string>("lmi_keeper_type", "3"));
	                        keyValuePairs.Add(new KeyValuePair<string, string>("choose_auth",
	                            "%CF%E5%F0%E5%E9%F2%E8+%EA+%EE%EF%EB%E0%F2%E5"));
	                        keyValuePairs.Add(new KeyValuePair<string, string>("ticket", group1.Value));
	                        keyValuePairs.Add(new KeyValuePair<string, string>("site", "ru"));
	                        keyValuePairs.Add(new KeyValuePair<string, string>("h", "340"));
	                        siteAsString =
	                            accountTool.PostWebmoney("https://merchant.webmoney.ru/lmi/NewSignedLoginAction2.asp?SP=",
	                                keyValuePairs);
	                        keyValuePairs.Clear();
	                        keyValuePairs.Add(new KeyValuePair<string, string>("c_purse", HbRelogManager.Settings.WmrPurse));
	                        keyValuePairs.Add(new KeyValuePair<string, string>("do_payment", "Платеж подтверждаю"));
	                        siteAsString = accountTool.PostWebmoney("https://merchant.webmoney.ru/lmi/payment_do.asp",
	                            keyValuePairs);
	                        if (siteAsString.Contains("Оплата выполнена"))
	                        {
	                            proxy.Status = "OK";
	                            AccountManager.AddAccount(proxy);
	                        }
	                    }
	                }
	                else
	                {
	                    MessageBox.Show("Cannot create account!");
	                }
	            }
	        }
	    }

        /// <summary>
        /// Генерация емайла
        /// </summary>
        /// <returns></returns>
		private static string GenerateEmail()
		{
			return string.Concat(GenerateRandomString(), "@mail.ru");
		}

        /// <summary>
        /// Генерация случайной последовательности букв
        /// </summary>
        /// <returns></returns>
		private static string GenerateRandomString()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.EnsureCapacity(10);
			var str = "qwertyuiopasdfghjklzxcvbnm";
			for (var i = 0; i < 10; i++)
			{
				stringBuilder.Append(str[random.Next(0, str.Length)]);
			}
			return stringBuilder.ToString();
		}
	}
}