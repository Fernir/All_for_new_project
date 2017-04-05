using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace HighVoltz.HBRelog.Source.AntiCaptcha
{
	public class AnticaptchaApiWrapper
	{
		public static Dictionary<string, bool> HostsChecked;

		static AnticaptchaApiWrapper()
		{
			AnticaptchaApiWrapper.HostsChecked = new Dictionary<string, bool>();
		}

		public AnticaptchaApiWrapper()
		{
		}

		public static bool CheckHost(string host)
		{
			if (!AnticaptchaApiWrapper.HostsChecked.ContainsKey(host))
			{
				AnticaptchaApiWrapper.HostsChecked[host] = AnticaptchaApiWrapper.Ping(host);
			}
			return AnticaptchaApiWrapper.HostsChecked[host];
		}

		public static AnticaptchaTask CreateImageToTextTask(string host, string clientKey, string pathToImageOrBase64Body, bool? phrase = null, bool? _case = null, int? numeric = null, bool? math = null, int? minLength = null, int? maxLength = null)
		{
			// 
			// Current member / type: HighVoltz.HBRelog.Source.AntiCaptcha.AnticaptchaTask HighVoltz.HBRelog.Source.AntiCaptcha.AnticaptchaApiWrapper::CreateImageToTextTask(System.String,System.String,System.String,System.Nullable`1<System.Boolean>,System.Nullable`1<System.Boolean>,System.Nullable`1<System.Int32>,System.Nullable`1<System.Boolean>,System.Nullable`1<System.Int32>,System.Nullable`1<System.Int32>)
			// File path: C:\Projects\бот игры\Project\Reloger\New_Reloger1\WoWRelogger.exe
			// 
			// Product version: 2016.1.316.0
			// Exception in: HighVoltz.HBRelog.Source.AntiCaptcha.AnticaptchaTask CreateImageToTextTask(System.String,System.String,System.String,System.Nullable<System.Boolean>,System.Nullable<System.Boolean>,System.Nullable<System.Int32>,System.Nullable<System.Boolean>,System.Nullable<System.Int32>,System.Nullable<System.Int32>)
			// 
			// Invalid argument: argumentInfo.
			//    в ..( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Steps\DynamicVariables\ResolveDynamicVariablesStep.cs:строка 329
			//    в ..( , FieldDefinition ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Steps\DynamicVariables\ResolveDynamicVariablesStep.cs:строка 134
			//    в ..( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Steps\DynamicVariables\ResolveDynamicVariablesStep.cs:строка 50
			//    в ..Visit( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 90
			//    в ..Visit(IEnumerable ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 383
			//    в ..( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 388
			//    в ..Visit( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 81
			//    в ..( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 507
			//    в ..Visit( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 132
			//    в ..Visit(IEnumerable ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 383
			//    в ..( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 388
			//    в ..Visit( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 81
			//    в ..( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 507
			//    в ..Visit( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 132
			//    в ..Visit(IEnumerable ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 383
			//    в ..( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 388
			//    в ..Visit( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 81
			//    в ..( ,  ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Steps\DynamicVariables\ResolveDynamicVariablesStep.cs:строка 30
			//    в ..(MethodBody ,  , ILanguage ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\DecompilationPipeline.cs:строка 88
			//    в ..(MethodBody , ILanguage ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\DecompilationPipeline.cs:строка 70
			//    в ..( , ILanguage , MethodBody , & ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\Extensions.cs:строка 99
			//    в ..(MethodBody , ILanguage , & ,  ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\Extensions.cs:строка 62
			//    в ..(ILanguage , MethodDefinition ,  ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\WriterContextServices\BaseWriterContextService.cs:строка 117
			// 
			// mailto: JustDecompilePublicFeedback@telerik.com
            //todo работа антикапчей
		    return null;
		}

		public static AnticaptchaTask CreateNoCaptchaTask(string host, string clientKey, string websiteUrl, string websiteKey, AnticaptchaApiWrapper.ProxyType proxyType, string proxyAddress, int proxyPort, string proxyLogin, string proxyPassword, string userAgent, string websiteSToken = "")
		{
			return AnticaptchaApiWrapper.CreateNoCaptchaTask("NoCaptchaTask", host, clientKey, websiteUrl, websiteKey, new AnticaptchaApiWrapper.ProxyType?(proxyType), proxyAddress, new int?(proxyPort), proxyLogin, proxyPassword, userAgent, websiteSToken);
		}

		private static AnticaptchaTask CreateNoCaptchaTask(string type, string host, string clientKey, string websiteUrl, string websiteKey, AnticaptchaApiWrapper.ProxyType? proxyType, string proxyAddress, int? proxyPort, string proxyLogin, string proxyPassword, string userAgent, string websiteSToken = "")
		{
			// 
			// Current member / type: HighVoltz.HBRelog.Source.AntiCaptcha.AnticaptchaTask HighVoltz.HBRelog.Source.AntiCaptcha.AnticaptchaApiWrapper::CreateNoCaptchaTask(System.String,System.String,System.String,System.String,System.String,System.Nullable`1<HighVoltz.HBRelog.Source.AntiCaptcha.AnticaptchaApiWrapper/ProxyType>,System.String,System.Nullable`1<System.Int32>,System.String,System.String,System.String,System.String)
			// File path: C:\Projects\бот игры\Project\Reloger\New_Reloger1\WoWRelogger.exe
			// 
			// Product version: 2016.1.316.0
			// Exception in: HighVoltz.HBRelog.Source.AntiCaptcha.AnticaptchaTask CreateNoCaptchaTask(System.String,System.String,System.String,System.String,System.String,System.Nullable<HighVoltz.HBRelog.Source.AntiCaptcha.AnticaptchaApiWrapper/ProxyType>,System.String,System.Nullable<System.Int32>,System.String,System.String,System.String,System.String)
			// 
			// Invalid argument: argumentInfo.
			//    в ..( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Steps\DynamicVariables\ResolveDynamicVariablesStep.cs:строка 329
			//    в ..( , FieldDefinition ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Steps\DynamicVariables\ResolveDynamicVariablesStep.cs:строка 134
			//    в ..( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Steps\DynamicVariables\ResolveDynamicVariablesStep.cs:строка 50
			//    в ..Visit( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 90
			//    в ..Visit(IEnumerable ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 383
			//    в ..( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 388
			//    в ..Visit( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 81
			//    в ..( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 507
			//    в ..Visit( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 132
			//    в ..Visit(IEnumerable ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 383
			//    в ..( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 388
			//    в ..Visit( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 81
			//    в ..( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 507
			//    в ..Visit( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 132
			//    в ..Visit(IEnumerable ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 383
			//    в ..( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 388
			//    в ..Visit( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 81
			//    в ..( ,  ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Steps\DynamicVariables\ResolveDynamicVariablesStep.cs:строка 30
			//    в ..(MethodBody ,  , ILanguage ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\DecompilationPipeline.cs:строка 88
			//    в ..(MethodBody , ILanguage ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\DecompilationPipeline.cs:строка 70
			//    в ..( , ILanguage , MethodBody , & ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\Extensions.cs:строка 99
			//    в ..(MethodBody , ILanguage , & ,  ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\Extensions.cs:строка 62
			//    в ..(ILanguage , MethodDefinition ,  ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\WriterContextServices\BaseWriterContextService.cs:строка 117
			// 
			// mailto: JustDecompilePublicFeedback@telerik.com
            //todo работа антикапчей
		    return null;
		}

		public static AnticaptchaTask CreateNoCaptchaTaskProxyless(string host, string clientKey, string websiteUrl, string websiteKey, string userAgent, string websiteSToken = "")
		{
			AnticaptchaApiWrapper.ProxyType? nullable = null;
			int? nullable1 = null;
			return AnticaptchaApiWrapper.CreateNoCaptchaTask("NoCaptchaTaskProxyless", host, clientKey, websiteUrl, websiteKey, nullable, null, nullable1, null, null, userAgent, websiteSToken);
		}

		public static AnticaptchaResult GetTaskResult(string host, string clientKey, AnticaptchaTask task)
		{
			// 
			// Current member / type: HighVoltz.HBRelog.Source.AntiCaptcha.AnticaptchaResult HighVoltz.HBRelog.Source.AntiCaptcha.AnticaptchaApiWrapper::GetTaskResult(System.String,System.String,HighVoltz.HBRelog.Source.AntiCaptcha.AnticaptchaTask)
			// File path: C:\Projects\бот игры\Project\Reloger\New_Reloger1\WoWRelogger.exe
			// 
			// Product version: 2016.1.316.0
			// Exception in: HighVoltz.HBRelog.Source.AntiCaptcha.AnticaptchaResult GetTaskResult(System.String,System.String,HighVoltz.HBRelog.Source.AntiCaptcha.AnticaptchaTask)
			// 
			// Invalid argument: argumentInfo.
			//    в ..( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Steps\DynamicVariables\ResolveDynamicVariablesStep.cs:строка 329
			//    в ..( , FieldDefinition ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Steps\DynamicVariables\ResolveDynamicVariablesStep.cs:строка 134
			//    в ..( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Steps\DynamicVariables\ResolveDynamicVariablesStep.cs:строка 50
			//    в ..Visit( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 90
			//    в ..Visit(IEnumerable ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 383
			//    в ..( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 388
			//    в ..Visit( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 81
			//    в ..( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 507
			//    в ..Visit( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 132
			//    в ..Visit(IEnumerable ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 383
			//    в ..( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 388
			//    в ..Visit( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 81
			//    в ..( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 507
			//    в ..Visit( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 132
			//    в ..Visit(IEnumerable ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 383
			//    в ..( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 388
			//    в ..Visit( ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:строка 81
			//    в ..( ,  ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Steps\DynamicVariables\ResolveDynamicVariablesStep.cs:строка 30
			//    в ..(MethodBody ,  , ILanguage ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\DecompilationPipeline.cs:строка 88
			//    в ..(MethodBody , ILanguage ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\DecompilationPipeline.cs:строка 70
			//    в ..( , ILanguage , MethodBody , & ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\Extensions.cs:строка 99
			//    в ..(MethodBody , ILanguage , & ,  ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\Extensions.cs:строка 62
			//    в ..(ILanguage , MethodDefinition ,  ) в c:\Builds\245\Behemoth\ReleaseBranch Production Build NT\Sources\OpenSource\Cecil.Decompiler\Decompiler\WriterContextServices\BaseWriterContextService.cs:строка 117
			// 
			// mailto: JustDecompilePublicFeedback@telerik.com
            //todo работа антикапчей
		    return null;
		}

		private static string ImagePathToBase64String(string path)
		{
			string base64String;
			try
			{
				using (Image image = Image.FromFile(path))
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						image.Save(memoryStream, image.RawFormat);
						base64String = Convert.ToBase64String(memoryStream.ToArray());
					}
				}
			}
			catch
			{
				base64String = null;
			}
			return base64String;
		}

		private static dynamic JsonPostRequest(string host, string methodName, string postData)
		{
			return HttpHelper.Post(new Uri(string.Concat("http://", host, "/", methodName)), postData);
		}

		public static bool Ping(string host)
		{
			bool flag;
			try
			{
				(new Ping()).Send(host, 1000);
				flag = true;
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		public enum ProxyType
		{
			http
		}
	}
}