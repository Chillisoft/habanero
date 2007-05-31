using System;
using System.Collections.Generic;
using Outlook;

namespace Chillisoft.OutlookUtils.v2

{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class AttachmentRipper
	{
		private Application itsOutlookApp;
		private _NameSpace itsOutlookNamespace;
		private MAPIFolder itsInbox;

		public AttachmentRipper()
		{
			itsOutlookApp = new Application();
			itsOutlookNamespace = itsOutlookApp.Session;
			itsInbox = itsOutlookNamespace.GetDefaultFolder(OlDefaultFolders.olFolderInbox);
		}


        public void RipAttachments(string[] extensions, string toPath)
		{
			MAPIFolder inboxArchiveFolder = getArchiveFolder();

            ItemsClass items = (ItemsClass)itsInbox.Items;
            //MailItem inboxItem = (MailItem)items.GetFirst();
            //do
			List<MailItem> mailItemsForMoving = new List<MailItem>();
        	foreach (MailItem inboxItem in items)
			{
				if (inboxItem != null && inboxItem.Attachments.Count > 0)
				{
					bool needsMoving = false;
					foreach (Attachment attachment in inboxItem.Attachments)
				    {
						foreach (string extension in extensions)
						{
							if (attachment.FileName.EndsWith(extension, StringComparison.CurrentCultureIgnoreCase))
							{
								Console.Out.WriteLine(" - Ripping '" + attachment.FileName + "' to '" + toPath + "'...");
								attachment.SaveAsFile(toPath + attachment.FileName);
								needsMoving = true;
								//Break out of the extensions loop for this attachment.
								break; 
							}
						}
				    }
                    //inboxItem.Move(archiveFolder);
					if(needsMoving)
					{
						mailItemsForMoving.Add(inboxItem);
					}
				}
				//inboxItem = (MailItem) items.GetNext();
			}// while (inboxItem != null);
        	foreach (MailItem mailItem in mailItemsForMoving)
        	{
        		mailItem.Move(inboxArchiveFolder);
        	}

		}

		private MAPIFolder getArchiveFolder()
		{
			_Folders folders = itsInbox.Folders;
			string archiveFolderName = "Archive";
			MAPIFolder archiveFolder = null;
			bool found = false;
			foreach (MAPIFolder folder in folders)
			{
				if (folder.Name == archiveFolderName)
				{
					found = true;
					archiveFolder = folder;
					break;
				}
			}
			if (!found)
			{
				archiveFolder = folders.Add(archiveFolderName, OlDefaultFolders.olFolderInbox);
			}
			return archiveFolder;
		}

		public void DeleteOldArchivedFiles(int daysBack)
		{
			MAPIFolder archiveFolder = getArchiveFolder();

			_Items items = archiveFolder.Items;
			MailItem archiveItem = (MailItem) items.GetFirst();
			do
			{
				if (archiveItem != null)
				{
					if (archiveItem.SentOn.AddDays(daysBack) < DateTime.Now)
					{
						archiveItem.Delete();
					}

				}
				archiveItem = (MailItem) items.GetNext();
			} while (archiveItem != null);


		}

		public void EmptyTrash()
		{
			_Items items;
			MAPIFolder trash = itsOutlookNamespace.GetDefaultFolder(OlDefaultFolders.olFolderDeletedItems);
			items = trash.Items;
			object item = items.GetFirst();
			do
			{
				if (item != null)
				{
					try
					{
						MailItem trashItem = (MailItem) item;
						trashItem.Delete();
					}
					catch (InvalidCastException)
					{
                        // Do nothing
					}
				}
				item = items.GetNext();
			} while (item != null);
		}
	}


}