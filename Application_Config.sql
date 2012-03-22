INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (5,'SiteMasterFirstName','Melih');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (10,'SiteMasterLastName','Gunal');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (15,'SiteMasterEmail','gunalmel@yahoo.com');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (20,'SiteMasterPhone','7349986145');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (25,'MinABusStudents','1');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (30,'MinAEngStudents','1');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (35,'MinAStudents','2');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (40,'MaxRejectedBusStudents','1');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (45,'MaxRejectedEngStudents','1');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (50,'MaxRejectedStudents','2');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (55,'RejectedStudentThreshold','5');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (56,'MinFirstProjects','1');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (60,'MaxRejectedProjects','1');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (65,'RejectedProjectThreshold','5');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (70,'EnforceContinuousProjectRanking','True');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (75,'EnforceContinuousStudentRanking','True');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (100,'MailServer','smtp.gmail.com');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (105,'MailServerPort','587');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (110,'IsSSLEnabled','True');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (115,'IsMailBodyHtml','True');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (120,'MailAccount','tauberprojects');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (125,'MailPassword','projectstauber');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (130,'IsTesting','False');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (135,'MailPickupDirectory','AppData/uploads');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (140,'RankProjectsController','/RankProjects/Index');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (145,'RankStudentsController','/RankStudents/Index');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (150,'InvalidAccessUrlMessage','Your access Url is invalid. Please contact Tauber Institute at The University Of Michigan for access.<br/><br/> Email:{0} <br/> PHONE:{1}');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (155,'ProjectAccessUrlEmailSubject','Tauber Matching Application - Url to rank students');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (160,'ProjectAccessUrlEmailBody','To rank Tauber Institute students for your project, please follow this link: <a href="{0}">{0}</a>');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (165,'StudentAccessUrlEmailSubject','Tauber Matching Application - Url to rank projects');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (170,'StudentAccessUrlEmailBody','To access Tauber Matching Web Application and rank projects, please follow this link: <a href="{0}">{0}</a>');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (175,'EmailHeader','Dear {0},<br /><br />
');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (180,'EmailFooter','<p>
	If you have any concerns about the ranking process or UM Tauber Matching Web Application, please free to contact {0} at:</p>
<p>
	Email: <a href="mailto:{1}?subject=UM Tauber Matching Web App - Question/Concern From User - {2}">{1}</a><br />
	Phone: {3}</p>
');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (185,'ConfirmationEmailReceivers','gunalmel@yahoo.com,gunalmel@gmail.com')
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (150,'InvalidAccessUrlMessage','Your access Url is invalid. Please contact Tauber Institute at The University Of Michigan for access.&lt;br/&gt;&lt;br/&gt; Email:{0} &lt;br/&gt; PHONE:{1}');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (155,'ProjectAccessUrlEmailSubject','Tauber Matching Application - Url to rank students');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (160,'ProjectAccessUrlEmailBody','To rank Tauber Institute students for your project, please follow this link: &lt;a href=&quot;{0}&quot;&gt;{0}&lt;/a&gt;');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (165,'StudentAccessUrlEmailSubject','Tauber Matching Application - Url to rank projects');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (170,'StudentAccessUrlEmailBody','To access Tauber Matching Web Application and rank projects, please follow this link: &lt;a href=&quot;{0}&quot;&gt;{0}&lt;/a&gt;');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (175,'EmailHeader','Dear {0}, &lt;br/&gt;&lt;br/&gt;');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (180,'EmailFooter','&lt;p&gt;If you have any concerns about the ranking process or UM Tauber Matching Web Application, please free to contact {0} at:&lt;/p&gt;&lt;p&gt;Email: &lt;a href=&quot;mailto:{1}?subject=UM Tauber Matching Web App - Question/Concern From User - {2}&quot;&gt;{1}&lt;/a&gt;&lt;br/&gt;Phone: {3}&lt;br/&gt;&lt;/p&gt;');
INSERT INTO [ConfigParameters] (Id,Name,Value) VALUES (185,'ConfirmationEmailReceivers','gunalmel@yahoo.com,sdemirel@gmail.com');

SET IDENTITY_INSERT ScoreDetails ON;
insert into ScoreDetails (Id,ScoreFor,Score,ScoreTypeDisplay) Values(0,'Project','NoScore','Not Ranked');
insert into ScoreDetails (Id,ScoreFor,Score,ScoreTypeDisplay) Values(10,'Project','A','A');
insert into ScoreDetails (Id,ScoreFor,Score,ScoreTypeDisplay) Values(20,'Project','B','B');
insert into ScoreDetails (Id,ScoreFor,Score,ScoreTypeDisplay) Values(30,'Project','C','C');
insert into ScoreDetails (Id,ScoreFor,Score,ScoreTypeDisplay) Values(40,'Project','Reject','Reject');
insert into ScoreDetails (Id,ScoreFor,Score,ScoreTypeDisplay) Values(50,'Student','NoScore','Not Ranked');
insert into ScoreDetails (Id,ScoreFor,Score,ScoreTypeDisplay) Values(60,'Student','1','First');
insert into ScoreDetails (Id,ScoreFor,Score,ScoreTypeDisplay) Values(70,'Student','2','Second');
insert into ScoreDetails (Id,ScoreFor,Score,ScoreTypeDisplay) Values(80,'Student','3','Third');
insert into ScoreDetails (Id,ScoreFor,Score,ScoreTypeDisplay) Values(90,'Student','4','Fourth');
insert into ScoreDetails (Id,ScoreFor,Score,ScoreTypeDisplay) Values(100,'Student','5','Fifth');
insert into ScoreDetails (Id,ScoreFor,Score,ScoreTypeDisplay) Values(110,'Student','Reject','Reject');
SET IDENTITY_INSERT ScoreDetails OFF;
INSERT INTO [Applications] (ApplicationName,ApplicationId,Description) VALUES ('tauberMatching',08a05d8b-df6b-4d91-bda4-c66e0bd35907,'');
INSERT INTO [Memberships] (ApplicationId,UserId,Password,PasswordFormat,PasswordSalt,Email,PasswordQuestion,PasswordAnswer,IsApproved,IsLockedOut,CreateDate,LastLoginDate,LastPasswordChangedDate,LastLockoutDate,FailedPasswordAttemptCount,FailedPasswordAttemptWindowStart,FailedPasswordAnswerAttemptCount,FailedPasswordAnswerAttemptWindowsStart,Comment) VALUES (08a05d8b-df6b-4d91-bda4-c66e0bd35907,d6cc2911-419b-4f43-8d2e-7436eb3b63ef,'YNfFFnFMCUfbUMSMwMPjvvE31yvm78o0pBHJzsq6+6Q=',1,'r9lnSSM/nPgaeF80LZG1FQ==','tauberprojects@gmail.con','','',True,False,12/29/2011 9:41:52 PM,12/30/2011 6:01:14 AM,12/29/2011 9:41:52 PM,1/1/1754 12:00:00 AM,0,1/1/1754 12:00:00 AM,0,1/1/1754 12:00:00 AM,'');
INSERT INTO [Roles] (ApplicationId,RoleId,RoleName,Description) VALUES (08a05d8b-df6b-4d91-bda4-c66e0bd35907,7b16edbc-4a2d-49fa-97c6-bf9f39716f60,'Administrator','');
INSERT INTO [Users] (ApplicationId,UserId,UserName,IsAnonymous,LastActivityDate) VALUES (08a05d8b-df6b-4d91-bda4-c66e0bd35907,d6cc2911-419b-4f43-8d2e-7436eb3b63ef,'admin',False,12/30/2011 6:01:14 AM);
INSERT INTO [UsersInRoles] (UserId,RoleId) VALUES (d6cc2911-419b-4f43-8d2e-7436eb3b63ef,7b16edbc-4a2d-49fa-97c6-bf9f39716f60);
