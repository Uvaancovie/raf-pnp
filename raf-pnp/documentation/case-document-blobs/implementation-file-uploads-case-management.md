Implement File Uploads for Case Management
User Review Required
IMPORTANT

Files will be stored in wwwroot/uploads for this iteration. Ensure write permissions are set for the application on this folder. No cloud storage (Azure Blob) is being implemented in this step, per the initial "file capture" request and local development context.

Proposed Changes
UI Changes
[MODIFY] 
Details.cshtml
Add a "Documents" section/tab.
Add an HTML form for uploading files:
File Input (<input type="file" ...>)
Document Type Dropdown (<select ...>)
Description Input (<input type="text" ...>)
Submit Button
Add a table/list to display uploaded documents for the current case.
Backend Changes
[MODIFY] 
Details.cshtml.cs
Add a bound property UploadPayload containing:
IFormFile File
DocumentType DocType
string Description
Add OnPostUploadAsync handler:
Validate file (not null, size limit).
Generate a secure unique filename.
Save file to wwwroot/uploads/cases/{CaseId}/.
Create 
CaseDocument
 entity.
Save changesto DB.
Redirect back to Details page.
Verification Plan
Manual Verification
Navigate to Case Details: Open the application, go to "Cases", and select a case.
Upload File:
Scroll to the "Documents" section.
Select a sample PDF or Image file.
Choose a "Document Type" (e.g., RAF 1 Form).
Enter a description.
Click "Upload".
Verify UI: Ensure the page reloads and shows the new document in the list.
Verify Storage: Check d:\raf-pnp\raf-pnp\wwwroot\uploads\cases\{CaseId}\ to see if the file exists.
Verify Database: Check if the record exists (implied by step 3).