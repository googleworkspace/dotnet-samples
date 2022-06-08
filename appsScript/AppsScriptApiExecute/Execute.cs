// @license
// Copyright Google Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// [START apps_script_api_execute]

using Google.Apis.Auth.OAuth2;
using Google.Apis.Script.v1;
using Google.Apis.Script.v1.Data;
using Google.Apis.Services;

namespace AppsScriptApiExecute
{
	class Program
	{
		static void Main(string[] args)
		{
			/* Load pre-authorized user credentials from the environment.
			TODO(developer) - See https://developers.google.com/identity for
			guides on implementing OAuth2 for your application. */
			GoogleCredential credential = GoogleCredential.GetApplicationDefault()
			.CreateScoped(ScriptService.Scope.ScriptDeployments);

			// Create Google Apps Script API service.
			string scriptId = "ENTER_YOUR_SCRIPT_ID_HERE";
			var service = new ScriptService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = "AppsScript API Execute",
			});

			// Create an execution request object.
			ExecutionRequest request = new ExecutionRequest
			{
				Function = "getFoldersUnderRoot"
			};

			ScriptsResource.RunRequest runReq =
				service.Scripts.Run(request, scriptId);

			try
			{
				// Make the API request.
				Operation op = runReq.Execute();

				if (op.Error != null)
				{
					// The API executed, but the script returned an error.

					// Extract the first (and only) set of error details
					// as a IDictionary. The values of this dictionary are
					// the script's 'errorMessage' and 'errorType', and an
					// array of stack trace elements. Casting the array as
					// a JSON JArray allows the trace elements to be accessed
					// directly.
					IDictionary<string, object> error = op.Error.Details[0];
					Console.WriteLine(
						"Script error message: {0}", error["errorMessage"]);
					{
						// There may not be a stacktrace if the script didn't
						// start executing.
						Console.WriteLine("Script error stacktrace:");
						Newtonsoft.Json.Linq.JArray st =
							(Newtonsoft.Json.Linq.JArray)error["scriptStackTraceElements"];
						foreach (var trace in st)
						{
							Console.WriteLine(
								"\t{0}: {1}",
								trace["function"],
								trace["lineNumber"]);
						}
					}
				}
				else
				{
					// The result provided by the API needs to be cast into
					// the correct type, based upon what types the Apps
					// Script function returns. Here, the function returns
					// an Apps Script Object with String keys and values.
					// It is most convenient to cast the return value as a JSON
					// JObject (folderSet).
					Newtonsoft.Json.Linq.JObject folderSet =
						(Newtonsoft.Json.Linq.JObject)op.Response["result"];
					if (folderSet.Count == 0)
					{
						Console.WriteLine("No folders returned!");
					}
					else
					{
						Console.WriteLine("Folders under your root folder:");
						foreach (var folder in folderSet)
						{
							Console.WriteLine(
								"\t{0} ({1})", folder.Value, folder.Key);
						}
					}
				}
			}
			catch (Google.GoogleApiException e)
			{
				// The API encountered a problem before the script
				// started executing.
				Console.WriteLine("Error calling API:\n{0}", e);
			}
		}
	}
}
// [END apps_script_api_execute]
