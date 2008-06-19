-- MySQL Administrator dump 1.4
--
-- ------------------------------------------------------
-- Server version	5.0.45-community-nt


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


--
-- Create schema habanero_test_branch_1_5
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ habanero_test_branch_1_5;
USE habanero_test_branch_1_5;

--
-- Table structure for table `habanero_test_branch_1_5`.`car`
--

DROP TABLE IF EXISTS `car`;
CREATE TABLE `car` (
  `Car_ID` varchar(38) default NULL,
  `Owner_Id` varchar(38) default NULL,
  `Car_Reg_No` varchar(50) default NULL,
  `Driver_FK1` varchar(50) default NULL,
  `Driver_FK2` varchar(50) default NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_5`.`car`
--

/*!40000 ALTER TABLE `car` DISABLE KEYS */;
/*!40000 ALTER TABLE `car` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_5`.`circle`
--

DROP TABLE IF EXISTS `circle`;
CREATE TABLE `circle` (
  `CircleID` char(38) default NULL,
  `Radius` int(10) unsigned NOT NULL default '0',
  `ShapeID` char(38) default NULL,
  `Colour` int(10) unsigned NOT NULL default '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_5`.`circle`
--

/*!40000 ALTER TABLE `circle` DISABLE KEYS */;
/*!40000 ALTER TABLE `circle` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_5`.`contact_person`
--

DROP TABLE IF EXISTS `contact_person`;
CREATE TABLE `contact_person` (
  `ContactPersonID` char(38) NOT NULL default '',
  `Surname` varchar(255) default NULL,
  `FirstName` varchar(255) default NULL,
  `EmailAddress` varchar(255) default NULL,
  `PhoneNumber` varchar(255) default NULL,
  `CellNumber` varchar(255) default NULL,
  `DateOfBirth` datetime default NULL,
  `DateLastUpdated` datetime default NULL,
  `UserLastUpdated` varchar(255) default NULL,
  `MachineLastUpdated` varchar(255) default NULL,
  `VersionNumber` int(11) default NULL,
  `PK2_Prop1` varchar(255) default NULL,
  `PK2_Prop2` varchar(255) default NULL,
  `PK3_Prop` varchar(255) default NULL,
  `OrganisationID` char(38) default NULL,
  `UserLocked` varchar(45) default NULL,
  `DateTimeLocked` datetime default NULL,
  `MachineLocked` varchar(45) default NULL,
  `OperatingSystemUserLocked` varchar(45) default NULL,
  `Locked` tinyint(1) default NULL,
  PRIMARY KEY  (`ContactPersonID`),
  UNIQUE KEY `Index_2` (`Surname`,`FirstName`),
  KEY `FK_contact_person_1` (`OrganisationID`),
  CONSTRAINT `FK_contact_person_1` FOREIGN KEY (`OrganisationID`) REFERENCES `organisation` (`OrganisationID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_5`.`contact_person`
--

/*!40000 ALTER TABLE `contact_person` DISABLE KEYS */;
INSERT INTO `contact_person` (`ContactPersonID`,`Surname`,`FirstName`,`EmailAddress`,`PhoneNumber`,`CellNumber`,`DateOfBirth`,`DateLastUpdated`,`UserLastUpdated`,`MachineLastUpdated`,`VersionNumber`,`PK2_Prop1`,`PK2_Prop2`,`PK3_Prop`,`OrganisationID`,`UserLocked`,`DateTimeLocked`,`MachineLocked`,`OperatingSystemUserLocked`,`Locked`) VALUES 
 ('{00DF3AAD-348C-4DAB-9A63-D4438445442F}','79db197cd06a4692ab96db7051d83482',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
 ('{1122848D-932C-417D-9C9C-D942ED88B7F2}','bcfc1e20-6ed9-4b97-b98e-fbdfb060a81f',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
 ('{5BB14EB5-E2AB-4799-9011-152A701B1063}','5a9963ff2af341df81d210136b2390e2',NULL,NULL,NULL,NULL,NULL,'2008-05-17 05:59:02','NT AUTHORITY\\SYSTEM','CORE1',2,NULL,NULL,'ID={5BB14EB5-E2AB-4799-9011-152A701B1063}',NULL,NULL,NULL,NULL,NULL,NULL),
 ('{66AA16CD-C14B-4451-BC1E-62632D955645}','UpdatedValue',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
 ('{6B924953-7A65-4CFB-B9F1-27105A5EAE0E}','375faedb-e576-46ec-83bc-95aa8e42cbe4',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `contact_person` (`ContactPersonID`,`Surname`,`FirstName`,`EmailAddress`,`PhoneNumber`,`CellNumber`,`DateOfBirth`,`DateLastUpdated`,`UserLastUpdated`,`MachineLastUpdated`,`VersionNumber`,`PK2_Prop1`,`PK2_Prop2`,`PK3_Prop`,`OrganisationID`,`UserLocked`,`DateTimeLocked`,`MachineLocked`,`OperatingSystemUserLocked`,`Locked`) VALUES 
 ('{8B24C7E5-6E60-43F3-B52C-6B3D4E0D0730}','f77f64ed0e244747bcf936e5c7404268',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
 ('{A631CB53-8C4B-41FD-94F2-393394B98028}','please delete me23d2bbdca30d49a7ab6f5339549cd788','fjdal;fjasdf',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
 ('{AF2F4144-65D7-401E-B6A8-58EC9540653C}','My Surname 1',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
 ('{C6BBF1D8-241C-4066-B210-958389B89CD9}','493233ad-c5df-4fe8-aa84-2752a5563336',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
 ('{FAC2C64F-9787-481B-9A3E-7368CB0FAFF3}','UpdatedValue',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `contact_person` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_5`.`contact_person_address`
--

DROP TABLE IF EXISTS `contact_person_address`;
CREATE TABLE `contact_person_address` (
  `AddressID` char(38) NOT NULL default '',
  `ContactPersonID` char(38) NOT NULL default '',
  `AddressLine1` varchar(255) default NULL,
  `AddressLine2` varchar(255) default NULL,
  `AddressLine3` varchar(255) default NULL,
  `AddressLine4` varchar(255) default NULL,
  `OrganisationID` char(38) default NULL,
  PRIMARY KEY  (`AddressID`),
  KEY `FK_contact_person_address_1` (`ContactPersonID`),
  CONSTRAINT `FK_contact_person_address_1` FOREIGN KEY (`ContactPersonID`) REFERENCES `contact_person` (`ContactPersonID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_5`.`contact_person_address`
--

/*!40000 ALTER TABLE `contact_person_address` DISABLE KEYS */;
INSERT INTO `contact_person_address` (`AddressID`,`ContactPersonID`,`AddressLine1`,`AddressLine2`,`AddressLine3`,`AddressLine4`,`OrganisationID`) VALUES 
 ('{31029BF8-1128-41D4-8979-D62D93268981}','{A631CB53-8C4B-41FD-94F2-393394B98028}',NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `contact_person_address` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_5`.`contactpersoncompositekey`
--

DROP TABLE IF EXISTS `contactpersoncompositekey`;
CREATE TABLE `contactpersoncompositekey` (
  `PK1_Prop1` varchar(50) default NULL,
  `PK1_Prop2` varchar(50) default NULL,
  `Surname` varchar(50) default NULL,
  `FirstName` varchar(50) default NULL,
  `DateOfBirth` datetime default NULL,
  `DateLastUpdated` datetime default NULL,
  `UserLastUpdated` varchar(50) default NULL,
  `MachineLastUpdated` varchar(50) default NULL,
  `VersionNumber` int(11) default NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_5`.`contactpersoncompositekey`
--

/*!40000 ALTER TABLE `contactpersoncompositekey` DISABLE KEYS */;
INSERT INTO `contactpersoncompositekey` (`PK1_Prop1`,`PK1_Prop2`,`Surname`,`FirstName`,`DateOfBirth`,`DateLastUpdated`,`UserLastUpdated`,`MachineLastUpdated`,`VersionNumber`) VALUES 
 ('{10EB69F4-C20E-4F53-AF13-ABBBA5608096}','{359171FD-D2BC-41ED-B59A-C460B7A41C0B}','Surname','NewFirstName','1969-01-29 00:00:00','2008-05-17 05:57:49',NULL,NULL,1),
 ('{DE447115-9176-4AF3-B511-3437BA486996}','{8A81E113-A1D6-46F9-A10E-39D9F3258F0E}','Vincent','Brad','1980-01-22 00:00:00','2008-05-17 05:57:49',NULL,NULL,1),
 ('addf9cdb-2fa4-4cf2-93d6-3770fa981ce91','a9388833-30ab-419e-8437-bad2a48bd7d7','Vincent',NULL,NULL,'2008-05-17 05:57:49',NULL,NULL,1),
 ('addf9cdb-2fa4-4cf2-93d6-3770fa981ce9','a9388833-30ab-419e-8437-bad2a48bd7d7','Vincent 2',NULL,NULL,'2008-05-17 05:57:49',NULL,NULL,1),
 ('{4ADD9F38-2975-4575-83BF-409D63CA9353}','{A9C06EC2-3629-41ED-8A2F-4C4F386CCF0D}','Vincent','Brad','1980-01-22 00:00:00','2008-05-17 05:57:49',NULL,NULL,1),
 ('{1F4521BF-D588-4778-B714-C857B9470593}','{13CF9249-3F74-4460-8D6F-F1AA8C322936}','Vincent','Brad','1980-01-22 00:00:00','2008-05-17 05:57:49',NULL,NULL,1),
 ('{6FB6C952-AB63-4A07-97DF-7385FE6AFE56}','{17DA5D4F-F855-4196-B1FC-37794957DA4C}','Vincent','Brad','1980-01-22 00:00:00','2008-05-17 05:57:49',NULL,NULL,1);
/*!40000 ALTER TABLE `contactpersoncompositekey` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_5`.`filledcircle`
--

DROP TABLE IF EXISTS `filledcircle`;
CREATE TABLE `filledcircle` (
  `FilledCircleID` char(38) default NULL,
  `Colour` int(10) unsigned NOT NULL default '0',
  `ShapeID` char(38) default NULL,
  `CircleID` char(38) default NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_5`.`filledcircle`
--

/*!40000 ALTER TABLE `filledcircle` DISABLE KEYS */;
/*!40000 ALTER TABLE `filledcircle` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_5`.`mockbo`
--

DROP TABLE IF EXISTS `mockbo`;
CREATE TABLE `mockbo` (
  `MockBOID` varchar(38) default NULL,
  `MockBOProp1` varchar(38) default NULL,
  `MockBOProp2` varchar(50) default NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_5`.`mockbo`
--

/*!40000 ALTER TABLE `mockbo` DISABLE KEYS */;
INSERT INTO `mockbo` (`MockBOID`,`MockBOProp1`,`MockBOProp2`) VALUES 
 ('{8A96146A-5787-4AF6-9A4A-F8349C753FE4}',NULL,NULL),
 ('{4F4E755C-FDCE-4135-A8D9-9704F9B88BD0}','{3848C504-0AAB-4159-A314-ECE2D96B093F}',NULL),
 ('{C59E44F6-3B45-4F4F-AB5D-ADDEABDB0CD6}','{246C509D-ACE5-4087-8B76-5CC4885E976D}',NULL),
 ('{20CA9733-B2E8-4E0E-86F5-240C1CF3C8C6}',NULL,NULL);
/*!40000 ALTER TABLE `mockbo` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_5`.`mybo`
--

DROP TABLE IF EXISTS `mybo`;
CREATE TABLE `mybo` (
  `MyBoID` varchar(255) NOT NULL default '',
  `TestProp` varchar(45) default NULL,
  `TestProp2` varchar(45) default NULL,
  `ShapeID` char(38) default NULL,
  PRIMARY KEY  (`MyBoID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_5`.`mybo`
--

/*!40000 ALTER TABLE `mybo` DISABLE KEYS */;
INSERT INTO `mybo` (`MyBoID`,`TestProp`,`TestProp2`,`ShapeID`) VALUES 
 ('{380D6849-91A2-4F8E-A561-20ECF6B3D2AA}','qwe','rty',NULL),
 ('{89A82E77-2541-4305-942F-F4069563B2FF}','TestValue','TestValue2',NULL),
 ('{EB000E4A-5CCE-49ED-80FB-FA4FFB2A802A}',NULL,NULL,'{13AF46E7-51AC-46F3-B62E-B1391FE7F7BB}');
/*!40000 ALTER TABLE `mybo` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_5`.`numbergenerator`
--

DROP TABLE IF EXISTS `numbergenerator`;
CREATE TABLE `numbergenerator` (
  `SequenceNumber` int(10) unsigned NOT NULL default '0',
  `NumberType` varchar(45) NOT NULL default '',
  `UserLocked` varchar(45) default NULL,
  `Locked` tinyint(1) default NULL,
  `MachineLocked` varchar(45) default NULL,
  `OperatingSystemUserLocked` varchar(45) default NULL,
  `DateTimeLocked` datetime default NULL,
  PRIMARY KEY  (`NumberType`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_5`.`numbergenerator`
--

/*!40000 ALTER TABLE `numbergenerator` DISABLE KEYS */;
INSERT INTO `numbergenerator` (`SequenceNumber`,`NumberType`,`UserLocked`,`Locked`,`MachineLocked`,`OperatingSystemUserLocked`,`DateTimeLocked`) VALUES 
 (2,'GeneratedNumber',NULL,NULL,NULL,NULL,NULL),
 (0,'tmp','NT AUTHORITY\\SYSTEM',0,'CORE1','NT AUTHORITY\\SYSTEM','2008-05-17 05:58:41');
/*!40000 ALTER TABLE `numbergenerator` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_5`.`organisation`
--

DROP TABLE IF EXISTS `organisation`;
CREATE TABLE `organisation` (
  `OrganisationID` char(38) NOT NULL default '',
  PRIMARY KEY  (`OrganisationID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_5`.`organisation`
--

/*!40000 ALTER TABLE `organisation` DISABLE KEYS */;
INSERT INTO `organisation` (`OrganisationID`) VALUES 
 ('{0186C093-D64C-428B-A48D-F45C26B6CABD}'),
 ('{027EC91D-240B-4C53-8E80-C1B4DBCD82BB}'),
 ('{12302B03-FF1F-4BF3-ADB6-D5DB1106F2FB}'),
 ('{1B206484-8079-402E-A81A-D14528502C1E}'),
 ('{23B8F1B6-790D-4ED5-8FF9-C4E230FE3B1B}'),
 ('{31444F40-82C4-43D1-9D55-28A3BF51DECF}'),
 ('{390A57B7-23B8-45A4-AA17-D91AFFEE13CD}'),
 ('{5EEAAF6F-F7AC-4CA2-BF9B-5A51E83FCAF3}'),
 ('{6CDCACD8-0A1B-415F-BF36-1CDC71A28D06}'),
 ('{82F2FD52-55EF-4CBB-8FE2-9C13491CAECC}'),
 ('{8D9313A2-FDB4-4BC9-A711-1D487B1F0D31}'),
 ('{911EA577-B0E6-4B19-9AC0-4D1E77EA1401}'),
 ('{A3C5B0B7-A0A2-4DA9-8AE3-7A7D15E3321E}'),
 ('{B3B3C4B9-E729-4F9E-B270-EDBD3BA89941}'),
 ('{B8D2E368-D648-4E55-BD8A-CD4E447A3A6C}'),
 ('{D3811BDF-C54D-46DC-95C9-AD8C2441380D}'),
 ('{DF004D6F-BD3A-406E-8D13-B022E745A99B}'),
 ('{E06B39F6-1027-44C1-AC1B-A3D661300ECC}');
/*!40000 ALTER TABLE `organisation` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_5`.`shape`
--

DROP TABLE IF EXISTS `shape`;
CREATE TABLE `shape` (
  `ShapeID` char(38) default NULL,
  `ShapeName` varchar(45) default NULL,
  `ShapeType` varchar(45) default NULL,
  `Radius` int(10) unsigned NOT NULL default '0',
  `Colour` int(10) unsigned NOT NULL default '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_5`.`shape`
--

/*!40000 ALTER TABLE `shape` DISABLE KEYS */;
/*!40000 ALTER TABLE `shape` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_5`.`stubdatabasetransaction`
--

DROP TABLE IF EXISTS `stubdatabasetransaction`;
CREATE TABLE `stubdatabasetransaction` (
  `id` varchar(255) default NULL,
  `name` varchar(255) default NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_5`.`stubdatabasetransaction`
--

/*!40000 ALTER TABLE `stubdatabasetransaction` DISABLE KEYS */;
/*!40000 ALTER TABLE `stubdatabasetransaction` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_5`.`table_engine`
--

DROP TABLE IF EXISTS `table_engine`;
CREATE TABLE `table_engine` (
  `ENGINE_ID` varchar(38) default NULL,
  `CAR_ID` varchar(38) default NULL,
  `ENGINE_NO` varchar(50) default NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_5`.`table_engine`
--

/*!40000 ALTER TABLE `table_engine` DISABLE KEYS */;
/*!40000 ALTER TABLE `table_engine` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_5`.`testautoinc`
--

DROP TABLE IF EXISTS `testautoinc`;
CREATE TABLE `testautoinc` (
  `testautoincid` int(10) unsigned NOT NULL auto_increment,
  `testfield` varchar(45) NOT NULL default '',
  PRIMARY KEY  (`testautoincid`)
) ENGINE=InnoDB AUTO_INCREMENT=468 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_5`.`testautoinc`
--

/*!40000 ALTER TABLE `testautoinc` DISABLE KEYS */;
INSERT INTO `testautoinc` (`testautoincid`,`testfield`) VALUES 
 (1,'testing'),
 (467,'testing 123');
/*!40000 ALTER TABLE `testautoinc` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_5`.`testtableread`
--

DROP TABLE IF EXISTS `testtableread`;
CREATE TABLE `testtableread` (
  `TestTableReadData` varchar(50) default NULL,
  `TestTableReadID` varchar(38) default NULL,
  UNIQUE KEY `Index_1` (`TestTableReadData`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_5`.`testtableread`
--

/*!40000 ALTER TABLE `testtableread` DISABLE KEYS */;
INSERT INTO `testtableread` (`TestTableReadData`,`TestTableReadID`) VALUES 
 ('Test',NULL),
 ('Test2',NULL);
/*!40000 ALTER TABLE `testtableread` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_5`.`transactionlog`
--

DROP TABLE IF EXISTS `transactionlog`;
CREATE TABLE `transactionlog` (
  `TransactionSequenceNo` int(11) NOT NULL auto_increment,
  `DateTimeUpdated` datetime default NULL,
  `WindowsUser` varchar(50) default NULL,
  `LogonUser` varchar(50) default NULL,
  `MachineName` varchar(50) default NULL,
  `BusinessObjectTypeName` varchar(50) default NULL,
  `CRUDAction` varchar(50) default NULL,
  `DirtyXML` mediumtext,
  PRIMARY KEY  (`TransactionSequenceNo`)
) ENGINE=InnoDB AUTO_INCREMENT=860 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_5`.`transactionlog`
--

/*!40000 ALTER TABLE `transactionlog` DISABLE KEYS */;
INSERT INTO `transactionlog` (`TransactionSequenceNo`,`DateTimeUpdated`,`WindowsUser`,`LogonUser`,`MachineName`,`BusinessObjectTypeName`,`CRUDAction`,`DirtyXML`) VALUES 
 (853,'2008-05-17 05:58:50','NT AUTHORITYSYSTEM','','CORE1','ContactPersonTransactionLogging','Created','<ContactPersonTransactionLogging ID=ContactPersonID={3a8a2ba1-5402-4b7f-95f5-84f2749fe088}><Properties><ContactPersonID><PreviousValue></PreviousValue><NewValue>{3A8A2BA1-5402-4B7F-95F5-84F2749FE088}</NewValue></ContactPersonID><Surname><PreviousValue></PreviousValue><NewValue>a6062e05-d404-407a-94e5-030e51ad1c56</NewValue></Surname><ContactPersonTransactionLogging>'),
 (854,'2008-05-17 05:58:50','NT AUTHORITYSYSTEM','','CORE1','ContactPersonTransactionLogging','Updated','<ContactPersonTransactionLogging ID=ContactPersonID={3a8a2ba1-5402-4b7f-95f5-84f2749fe088}><Properties><Surname><PreviousValue>a6062e05-d404-407a-94e5-030e51ad1c56</PreviousValue><NewValue>5b2b9990-183f-4d92-ba17-31edf17551b5</NewValue></Surname><ContactPersonTransactionLogging>'),
 (855,'2008-05-17 05:58:50','NT AUTHORITYSYSTEM','','CORE1','ContactPersonTransactionLogging','Updated','<ContactPersonTransactionLogging ID=ContactPersonID={3a8a2ba1-5402-4b7f-95f5-84f2749fe088}><Properties><Surname><PreviousValue>5b2b9990-183f-4d92-ba17-31edf17551b5</PreviousValue><NewValue>75a994b7-8685-4f4f-ba52-8363fcd9becc</NewValue></Surname><ContactPersonTransactionLogging>');
INSERT INTO `transactionlog` (`TransactionSequenceNo`,`DateTimeUpdated`,`WindowsUser`,`LogonUser`,`MachineName`,`BusinessObjectTypeName`,`CRUDAction`,`DirtyXML`) VALUES 
 (856,'2008-05-17 05:58:50','NT AUTHORITYSYSTEM','','CORE1','ContactPersonTransactionLogging','Updated','<ContactPersonTransactionLogging ID=ContactPersonID={3a8a2ba1-5402-4b7f-95f5-84f2749fe088}><Properties><Surname><PreviousValue>75a994b7-8685-4f4f-ba52-8363fcd9becc</PreviousValue><NewValue>320c434c-b30d-42c4-ba4d-f311b4771f98</NewValue></Surname><ContactPersonTransactionLogging>'),
 (857,'2008-05-17 05:58:50','NT AUTHORITYSYSTEM','','CORE1','ContactPersonTransactionLogging','Updated','<ContactPersonTransactionLogging ID=ContactPersonID={3a8a2ba1-5402-4b7f-95f5-84f2749fe088}><Properties><Surname><PreviousValue>320c434c-b30d-42c4-ba4d-f311b4771f98</PreviousValue><NewValue>262d23c4-ebed-4b02-b4ee-4bcaf5e15fe0</NewValue></Surname><ContactPersonTransactionLogging>'),
 (858,'2008-05-17 05:58:50','NT AUTHORITYSYSTEM','','CORE1','ContactPersonTransactionLogging','Deleted','<ContactPersonTransactionLogging ID=ContactPersonID={3a8a2ba1-5402-4b7f-95f5-84f2749fe088}><Properties><ContactPersonTransactionLogging>'),
 (859,'2008-05-17 05:58:50','NT AUTHORITYSYSTEM','','CORE1','ContactPersonTransactionLogging','Created','<ContactPersonTransactionLogging ID=ContactPersonID={af2f4144-65d7-401e-b6a8-58ec9540653c}><Properties><ContactPersonID><PreviousValue></PreviousValue><NewValue>{AF2F4144-65D7-401E-B6A8-58EC9540653C}</NewValue></ContactPersonID><Surname><PreviousValue></PreviousValue><NewValue>My Surname 1</NewValue></Surname><ContactPersonTransactionLogging>');
/*!40000 ALTER TABLE `transactionlog` ENABLE KEYS */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
