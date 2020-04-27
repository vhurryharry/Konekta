using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WCA.AzureFunctions.EmailToSMS
{
    public static class MicrosoftGraphExtensions
    {
        public static async Task<MailFolder> EnsureFolder(this IUserRequestBuilder userRequestBuilder, MailFolder parentFolder, string folderName) =>
            await EnsureFolder(userRequestBuilder, parentFolder?.Id, folderName);

        public static async Task<MailFolder> EnsureFolder(this IUserRequestBuilder userRequestBuilder, string parentFolderId, string folderName)
        {
            if (userRequestBuilder is null) throw new ArgumentNullException(nameof(userRequestBuilder));

            var invalidMessagesFolderResults = await userRequestBuilder
                .MailFolders[parentFolderId]
                .ChildFolders
                .Request()
                .Filter($"displayName eq '{folderName}'")
                .GetAsync();

            if (invalidMessagesFolderResults.CurrentPage.Count > 0)
            {
                return invalidMessagesFolderResults[0];
            }
            else
            {
                var newFolder = await userRequestBuilder
                    .MailFolders[parentFolderId]
                    .ChildFolders
                    .Request()
                    .AddAsync(new MailFolder()
                    {
                        DisplayName = folderName
                    });

                return newFolder;
            }
        }

        public static async Task<Message> AddMessageCategoriesAsync(this GraphServiceClient graphServiceClient, string user, Message message, params string[] categoriesToAdd)
        {
            if (graphServiceClient is null) throw new ArgumentNullException(nameof(graphServiceClient));
            if (message is null) throw new ArgumentNullException(nameof(message));

            if (categoriesToAdd is null)
            {
                return message;
            }

            var newCategories = new List<string>(message.Categories);
            foreach (var categoryToAdd in categoriesToAdd)
            {
                if (!newCategories.Contains(categoryToAdd))
                {
                    newCategories.Add(categoryToAdd);
                }
            }

            var updateMessage = new Message();
            updateMessage.Categories = newCategories;
            
            return await graphServiceClient.Users[user].Messages[message.Id].Request().UpdateAsync(updateMessage);
        }
    }
}