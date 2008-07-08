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
-- Create schema habanero_test_trunk
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ habanero_test_trunk;
USE habanero_test_trunk;

--
-- Table structure for table `habanero_test_trunk`.`car`
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
-- Dumping data for table `habanero_test_trunk`.`car`
--

/*!40000 ALTER TABLE `car` DISABLE KEYS */;
INSERT INTO `car` (`Car_ID`,`Owner_Id`,`Car_Reg_No`,`Driver_FK1`,`Driver_FK2`) VALUES 
 ('{D108CE2E-F804-4DE6-8962-5A32B8C8C470}',NULL,'NP32459',NULL,NULL);
/*!40000 ALTER TABLE `car` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_trunk`.`circle`
--

DROP TABLE IF EXISTS `circle`;
CREATE TABLE `circle` (
  `CircleID` char(38) default NULL,
  `Radius` int(10) unsigned NOT NULL default '0',
  `ShapeID` char(38) default NULL,
  `Colour` int(10) unsigned NOT NULL default '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_trunk`.`circle`
--

/*!40000 ALTER TABLE `circle` DISABLE KEYS */;
/*!40000 ALTER TABLE `circle` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_trunk`.`circle_concrete`
--

DROP TABLE IF EXISTS `circle_concrete`;
CREATE TABLE `circle_concrete` (
  `CircleID` char(38) NOT NULL default '',
  `Radius` int(10) unsigned default NULL,
  `ShapeID` char(38) default NULL,
  `ShapeName` varchar(255) default NULL,
  PRIMARY KEY  (`CircleID`),
  KEY `Index_2` (`ShapeID`),
  CONSTRAINT `FK_circle_concrete_1` FOREIGN KEY (`ShapeID`) REFERENCES `shape` (`ShapeID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_trunk`.`circle_concrete`
--

/*!40000 ALTER TABLE `circle_concrete` DISABLE KEYS */;
/*!40000 ALTER TABLE `circle_concrete` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_trunk`.`contact_person`
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
-- Dumping data for table `habanero_test_trunk`.`contact_person`
--

/*!40000 ALTER TABLE `contact_person` DISABLE KEYS */;
INSERT INTO `contact_person` (`ContactPersonID`,`Surname`,`FirstName`,`EmailAddress`,`PhoneNumber`,`CellNumber`,`DateOfBirth`,`DateLastUpdated`,`UserLastUpdated`,`MachineLastUpdated`,`VersionNumber`,`PK2_Prop1`,`PK2_Prop2`,`PK3_Prop`,`OrganisationID`,`UserLocked`,`DateTimeLocked`,`MachineLocked`,`OperatingSystemUserLocked`,`Locked`) VALUES 
 ('{6219B991-515A-4368-9651-4BCBE6C23631}','66e6d8ac489d4acd80d1a4ade3a1d67c',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `contact_person` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_trunk`.`contact_person_address`
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
-- Dumping data for table `habanero_test_trunk`.`contact_person_address`
--

/*!40000 ALTER TABLE `contact_person_address` DISABLE KEYS */;
/*!40000 ALTER TABLE `contact_person_address` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_trunk`.`contactpersoncompositekey`
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
-- Dumping data for table `habanero_test_trunk`.`contactpersoncompositekey`
--

/*!40000 ALTER TABLE `contactpersoncompositekey` DISABLE KEYS */;
INSERT INTO `contactpersoncompositekey` (`PK1_Prop1`,`PK1_Prop2`,`Surname`,`FirstName`,`DateOfBirth`,`DateLastUpdated`,`UserLastUpdated`,`MachineLastUpdated`,`VersionNumber`) VALUES 
 ('{623A7178-E60F-4EEF-BB58-A3186F42C44E}','{C05B5FA4-FBA0-483E-9054-634E427DED5E}','Surname','NewFirstName','1969-01-29 00:00:00','2008-07-08 16:42:21',NULL,NULL,1),
 ('{A7DF4F8B-DE69-42AE-94B0-8FEDEBCAB2E0}','{87102925-3631-45C7-904C-1AC721A094A7}','Vincent','Brad','1980-01-22 00:00:00','2008-07-08 16:42:21',NULL,NULL,1),
 ('81d56f85-2b00-4cec-b7bd-d13f24ae2add1','85b58c34-8310-47fc-b897-1388a0796d3c','Vincent',NULL,NULL,'2008-07-08 16:42:21',NULL,NULL,1),
 ('81d56f85-2b00-4cec-b7bd-d13f24ae2add','85b58c34-8310-47fc-b897-1388a0796d3c','Vincent 2',NULL,NULL,'2008-07-08 16:42:21',NULL,NULL,1),
 ('{E4479C78-940C-4DEA-9C30-D5BAD20E3102}','{A22820AB-3C5C-4B4B-89F8-8352E604B01A}','Vincent','Brad','1980-01-22 00:00:00','2008-07-08 16:42:21',NULL,NULL,1),
 ('{A10EDA7D-905E-4732-95BB-90FBBE61B94B}','{A20631FF-CC6E-4771-BEF3-471984A23A1A}','Vincent','Brad','1980-01-22 00:00:00','2008-07-08 16:42:21',NULL,NULL,1),
 ('{85E82EC8-B468-4D90-8BC6-A4E1A8987723}','{833FC938-875D-464E-9A26-0048B3488520}','Vincent','Brad','1980-01-22 00:00:00','2008-07-08 16:42:21',NULL,NULL,1);
/*!40000 ALTER TABLE `contactpersoncompositekey` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_trunk`.`filledcircle`
--

DROP TABLE IF EXISTS `filledcircle`;
CREATE TABLE `filledcircle` (
  `FilledCircleID` char(38) default NULL,
  `Colour` int(10) unsigned NOT NULL default '0',
  `ShapeID` char(38) default NULL,
  `CircleID` char(38) default NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_trunk`.`filledcircle`
--

/*!40000 ALTER TABLE `filledcircle` DISABLE KEYS */;
/*!40000 ALTER TABLE `filledcircle` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_trunk`.`filledcircle_concrete`
--

DROP TABLE IF EXISTS `filledcircle_concrete`;
CREATE TABLE `filledcircle_concrete` (
  `FilledCircleID` char(38) NOT NULL default '',
  `Colour` int(10) unsigned default NULL,
  `ShapeID` char(38) default NULL,
  `CircleID` char(38) default NULL,
  `Radius` int(10) unsigned default NULL,
  `ShapeName` varchar(255) default NULL,
  PRIMARY KEY  (`FilledCircleID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `habanero_test_trunk`.`filledcircle_concrete`
--

/*!40000 ALTER TABLE `filledcircle_concrete` DISABLE KEYS */;
/*!40000 ALTER TABLE `filledcircle_concrete` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_trunk`.`mockbo`
--

DROP TABLE IF EXISTS `mockbo`;
CREATE TABLE `mockbo` (
  `MockBOID` varchar(38) default NULL,
  `MockBOProp1` varchar(38) default NULL,
  `MockBOProp2` varchar(50) default NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_trunk`.`mockbo`
--

/*!40000 ALTER TABLE `mockbo` DISABLE KEYS */;
INSERT INTO `mockbo` (`MockBOID`,`MockBOProp1`,`MockBOProp2`) VALUES 
 ('{8A96146A-5787-4AF6-9A4A-F8349C753FE4}',NULL,NULL),
 ('{4F4E755C-FDCE-4135-A8D9-9704F9B88BD0}','{3848C504-0AAB-4159-A314-ECE2D96B093F}',NULL),
 ('{C59E44F6-3B45-4F4F-AB5D-ADDEABDB0CD6}','{246C509D-ACE5-4087-8B76-5CC4885E976D}',NULL),
 ('{20CA9733-B2E8-4E0E-86F5-240C1CF3C8C6}',NULL,NULL),
 ('{6EA89DA3-6AE3-422D-B8BC-8BAABAF1441C}',NULL,NULL),
 ('{FFCA1B12-7D12-4719-90CE-8E56DA1F0614}','{B85E4A4A-07FF-4CB4-9051-1E62B442BB87}',NULL),
 ('{F3C51E2F-D472-4D53-8244-BFDDA1CE52F1}',NULL,NULL),
 ('{62C93F48-99F4-4422-BD64-93B1A8D3F3BE}','{EE8624C5-C991-4C4D-8244-66C2BB48431D}',NULL),
 ('{2DF54E6D-31F0-471F-8991-22E5514A838B}',NULL,NULL),
 ('{73E8B788-50F2-4BCD-87E6-A269A0B012AB}','{0D3FD3A1-D07A-4A1E-A3A3-FA837D054834}',NULL),
 ('{D4B4C8FC-088F-4419-BD3C-04AE95889ECA}',NULL,NULL),
 ('{AB750587-FCC7-4EE8-A561-E6E663181658}','{C7AEC131-BA15-44F7-9CA5-EA206A5D394D}',NULL),
 ('{17017730-1EE0-4402-8FA5-5FB69A193BA9}',NULL,NULL),
 ('{297B4AB1-FD96-4A46-BF04-066815CCF262}','{0E22AEDA-A134-48A7-A651-E560CBCA2604}',NULL);
INSERT INTO `mockbo` (`MockBOID`,`MockBOProp1`,`MockBOProp2`) VALUES 
 ('{A98BD55C-4D5C-4734-B4D1-7F4E3EF14F5E}',NULL,NULL),
 ('{90A20C37-C15C-41AF-9C12-799E93FA692D}','{58FC38B4-E3C3-4100-B73B-8097FD29F4BF}',NULL),
 ('{7EBDC49B-74C5-4903-80B6-3BD5126F226A}',NULL,NULL),
 ('{DA9F7084-BE34-4EF2-9B31-C93884601488}','{4F5F8B08-F0F6-4119-9C6B-78F09ED18140}',NULL),
 ('{7F5653C3-02BF-4C17-BA78-27DCD927394A}',NULL,NULL),
 ('{C80BBAA4-EE7A-4D02-A900-F8C66FFEF07C}','{DD4DD1DF-23ED-4F76-A8E1-1508A5B0A5F8}',NULL),
 ('{23652261-471B-40FA-9E46-B214BC83EF95}',NULL,NULL),
 ('{8E3B3DB8-319D-4A04-B919-05AF4DBAD9A8}','{BC98192E-091B-4587-B09F-3113FCE08616}',NULL),
 ('{1CAE65C7-874A-4708-A6B8-F3A2A87B086C}',NULL,NULL),
 ('{B52C5A5F-975A-4882-B4F0-3271D093D9A8}','{0F209822-4201-4BC8-8204-6F444CF5A3AD}',NULL),
 ('{C90FF506-4E6C-4927-95C0-29934CAA0618}',NULL,NULL),
 ('{F1AFF6EB-135C-4E66-A09B-2449ABF13699}','{90872F02-D822-43D2-AA9C-4283FE2702C9}',NULL),
 ('{41C649D1-9A48-43DA-B9F7-E17A28857956}',NULL,NULL),
 ('{D1D03798-9E0B-4262-A0D1-5AFE7DC3F883}','{4F625ADF-1C8A-4C28-80CA-B53DAB6BA06A}',NULL);
INSERT INTO `mockbo` (`MockBOID`,`MockBOProp1`,`MockBOProp2`) VALUES 
 ('{9B520A7F-2DC6-4F41-8256-2934B3949FDB}',NULL,NULL),
 ('{10C1AFE2-6C90-4D89-B902-C226335DCA6A}','{D91FBF7D-FEBF-4022-92F3-84AC2761B39A}',NULL),
 ('{C7309EE0-2611-4A68-8078-15D33864388F}',NULL,NULL),
 ('{4BE9ACB7-6AA3-41B4-BA93-EC21E16935D2}','{4D1DC47D-BA7D-41BD-85CE-973B6433E677}',NULL),
 ('{48CCD02B-DB9C-4741-BDC1-D760CE215D20}',NULL,NULL),
 ('{A49EE634-2D85-45B4-9D0F-01874EADA1AE}',NULL,NULL),
 ('{EBECA229-233F-4467-957A-7BBA0E8B70FD}','{B17AB8B5-3FEF-4CC3-9507-368AD67F8CE0}',NULL),
 ('{74590B1F-EA01-49FF-AE62-F1AF296BCE57}',NULL,NULL),
 ('{4AF11916-F434-44CA-B072-8D983B2CD87E}','{C35759FD-8A40-44DC-9B85-87D448B01DC6}',NULL),
 ('{BBB1A650-13BE-4A45-A7C0-91FD97D6F66E}',NULL,NULL),
 ('{22594225-25A5-4D20-B2EB-AC0CF057B822}',NULL,NULL),
 ('{9DAF596F-7554-4C36-96BF-C41EF50C4AD3}','{1FB2C870-9609-4510-828E-39FB439FC4E7}',NULL),
 ('{431415FF-B94B-4A43-9A27-2382ABA108D7}',NULL,NULL),
 ('{C1469290-886A-4ABF-8F97-794DC1432D0F}','{965D2E0D-910C-49CA-B6A0-BC69BA0757BF}',NULL),
 ('{712FB5A7-4298-486F-92E0-848A26DC09F0}',NULL,NULL);
INSERT INTO `mockbo` (`MockBOID`,`MockBOProp1`,`MockBOProp2`) VALUES 
 ('{3F31A760-04F6-4184-A6D5-8AFE101DB3C4}',NULL,NULL),
 ('{43C1B062-94D7-47DE-92EC-8F7E50547966}','{52F28DED-D441-4C0D-BCF3-6DC4594BA7A5}',NULL),
 ('{8127DFF0-67F3-4AA6-AB71-C86839E5FAAA}',NULL,NULL),
 ('{B4C276EA-BD2C-4BE6-8C23-EF5770E17DAE}','{A7AB783B-434C-4481-A618-B058D261C09B}',NULL),
 ('{C065AE66-7220-44FA-B322-CE97325D29C3}',NULL,NULL),
 ('{370B8268-AA83-41BC-87A9-BAF4F088E2B4}',NULL,NULL),
 ('{4384E9E0-6C5C-4495-A8BD-6046590DA72A}','{8F73A89B-ECF9-4D1E-B81A-55C75AAD93BD}',NULL),
 ('{E7E89542-2E0D-407A-BD91-D2DA958871B1}',NULL,NULL),
 ('{59054FA0-65E2-4156-A645-41837DCAE73A}','{2C0DC495-338F-4886-BDE7-1E83051A463A}',NULL),
 ('{CD150EAE-B1AD-4D6C-AB9F-2232B7E39138}',NULL,NULL),
 ('{81009C9C-F6DA-4E22-884B-D33E938D9766}',NULL,NULL),
 ('{657A5FD0-E3C7-4023-B8EA-55E86541DB47}','{190199AF-DFEB-41CE-B0BF-1F5D1119FA07}',NULL),
 ('{215435CE-5C21-4A01-8A9F-365B28FD44DE}','{3F8080BA-E316-4B72-A3C9-562D386B8CA0}',NULL),
 ('{A2A6F646-B317-40F8-818D-9492C4568B0A}',NULL,NULL);
/*!40000 ALTER TABLE `mockbo` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_trunk`.`mybo`
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
-- Dumping data for table `habanero_test_trunk`.`mybo`
--

/*!40000 ALTER TABLE `mybo` DISABLE KEYS */;
INSERT INTO `mybo` (`MyBoID`,`TestProp`,`TestProp2`,`ShapeID`) VALUES 
 ('{0339ED51-11E4-440A-B6D4-38E2D516B968}',NULL,NULL,'{71D60CD7-5CC5-4ED3-AF3E-0FF865F3E247}'),
 ('{0BCECB91-90F9-4A34-BF07-172C4785863B}','qwe','rty',NULL),
 ('{1092509E-DBA7-4C0B-BF60-65660EBDEB9E}','TestValue','TestValue2',NULL),
 ('{276658A3-0672-4858-B1DB-231A79A32EDF}',NULL,NULL,'{15E25589-1A16-4DB4-B909-40D7A83349BE}'),
 ('{2B23B008-F14B-4876-8270-75986AF85A6D}','TestValue','TestValue2',NULL),
 ('{2B58BF6A-3184-4DA6-AF7C-0119A9D05CC0}','qwe','rty',NULL),
 ('{33259EF8-0782-4EF4-AEB5-5FAB57414283}','qwe','rty',NULL),
 ('{347D4036-07E0-4F20-AB10-A59D354BFDAA}','TestValue','TestValue2',NULL),
 ('{380D6849-91A2-4F8E-A561-20ECF6B3D2AA}','qwe','rty',NULL),
 ('{434EC739-ECDC-4203-8509-C105A03DAEEA}','qwe','rty',NULL),
 ('{458CF3D9-A0A1-40DE-BC92-FE9515B4CAE8}','TestValue','TestValue2',NULL),
 ('{4A83B585-EF8E-4ED6-AD96-F38B36AD007F}','qwe','rty',NULL),
 ('{555580E1-5F97-45DC-A69B-68522E7B5EBD}','TestValue','TestValue2',NULL),
 ('{556316C6-F21F-419F-93D2-A6F28CAF3F96}','qwe','rty',NULL);
INSERT INTO `mybo` (`MyBoID`,`TestProp`,`TestProp2`,`ShapeID`) VALUES 
 ('{58BFAD39-AF56-47F1-AA9A-64584B43366D}',NULL,NULL,'{17113657-9590-4A59-B4E6-4F43E6EA6F63}'),
 ('{5AF8B03C-D41D-4120-9987-6E382F28D43A}','TestValue','TestValue2',NULL),
 ('{5B471A24-B919-4043-B5C8-76ACF40243B7}','TestValue','TestValue2',NULL),
 ('{5F875157-7287-47C0-8508-AB17CEFD9A4A}',NULL,NULL,'{9DAE3A66-4A33-4C5A-8D0E-5EA5EB59B796}'),
 ('{7B9672A7-446C-4619-A3C4-87015E3693FE}','TestValue','TestValue2',NULL),
 ('{84E43A45-7E05-4FC2-BC26-78AA09AE1269}',NULL,NULL,'{9EDF4E83-F505-495F-ADD9-7EEA1157C98D}'),
 ('{867C0713-7C35-4E5C-AD7D-C55C6970BA18}',NULL,NULL,'{28C77489-AEE0-48ED-8AB1-A666DEECFFA1}'),
 ('{89A82E77-2541-4305-942F-F4069563B2FF}','TestValue','TestValue2',NULL),
 ('{8E75AF97-FAFB-4D79-9502-B82411A59F65}','TestValue','TestValue2',NULL),
 ('{96EB0BF6-2DBC-4DEE-8249-80EE0671ADDC}','qwe','rty',NULL),
 ('{A7405866-1FC5-436D-8988-86B789EE85FC}','qwe','rty',NULL),
 ('{B0AB8152-73D8-4749-A008-A4A497A8A4A1}',NULL,NULL,'{7BEE1972-7D7B-41CF-BBDF-09CFB2E25030}'),
 ('{C2DC31F0-CC02-4F1B-B2FE-C03FB55E5B5A}',NULL,NULL,'{F872DA60-DB2A-4FED-9AC3-9897CE18BDD0}');
INSERT INTO `mybo` (`MyBoID`,`TestProp`,`TestProp2`,`ShapeID`) VALUES 
 ('{CC8927F0-BA79-481D-BE40-797183ED8F38}',NULL,NULL,'{605BB5BA-F733-4BA9-85A5-D1E0165B20A9}'),
 ('{CF3E5C4C-CE6F-456D-A6B0-AABB971489C4}',NULL,NULL,'{78D739E1-8A16-4423-AD52-DC588DEDF2C9}'),
 ('{CF4C814D-02CE-4F5E-B61B-840AF5CE3C63}','qwe','rty',NULL),
 ('{DBC79BCA-FCE3-46A4-8410-C2CCCFB977EA}','qwe','rty',NULL),
 ('{E40FDD73-1B1B-4F69-8276-0A9C9C1D350F}','TestValue','TestValue2',NULL),
 ('{E5FC5F53-669E-4A6D-A387-6AD87FBB76D5}',NULL,NULL,'{AF4338E7-45A9-44CA-82C6-261AD69FC3A7}'),
 ('{EAE21424-7FEF-4E27-9692-4F0DB514671C}','TestValue','TestValue2',NULL),
 ('{EB000E4A-5CCE-49ED-80FB-FA4FFB2A802A}',NULL,NULL,'{13AF46E7-51AC-46F3-B62E-B1391FE7F7BB}'),
 ('{F8E9FD56-06D4-4CF8-ADB4-6E7A8731C869}','qwe','rty',NULL);
/*!40000 ALTER TABLE `mybo` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_trunk`.`numbergenerator`
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
-- Dumping data for table `habanero_test_trunk`.`numbergenerator`
--

/*!40000 ALTER TABLE `numbergenerator` DISABLE KEYS */;
INSERT INTO `numbergenerator` (`SequenceNumber`,`NumberType`,`UserLocked`,`Locked`,`MachineLocked`,`OperatingSystemUserLocked`,`DateTimeLocked`) VALUES 
 (2,'GeneratedNumber',NULL,NULL,NULL,NULL,NULL),
 (0,'tmp','NT AUTHORITY\\SYSTEM',0,'CORE1','NT AUTHORITY\\SYSTEM','2008-07-08 16:41:44');
/*!40000 ALTER TABLE `numbergenerator` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_trunk`.`organisation`
--

DROP TABLE IF EXISTS `organisation`;
CREATE TABLE `organisation` (
  `OrganisationID` char(38) NOT NULL default '',
  PRIMARY KEY  (`OrganisationID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_trunk`.`organisation`
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
-- Table structure for table `habanero_test_trunk`.`shape`
--

DROP TABLE IF EXISTS `shape`;
CREATE TABLE `shape` (
  `ShapeID` char(38) NOT NULL default '',
  `ShapeName` varchar(45) default NULL,
  `ShapeType` varchar(45) default NULL,
  `Radius` int(10) unsigned NOT NULL default '0',
  `Colour` int(10) unsigned NOT NULL default '0',
  PRIMARY KEY  (`ShapeID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_trunk`.`shape`
--

/*!40000 ALTER TABLE `shape` DISABLE KEYS */;
/*!40000 ALTER TABLE `shape` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_trunk`.`stubdatabasetransaction`
--

DROP TABLE IF EXISTS `stubdatabasetransaction`;
CREATE TABLE `stubdatabasetransaction` (
  `id` varchar(255) default NULL,
  `name` varchar(255) default NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_trunk`.`stubdatabasetransaction`
--

/*!40000 ALTER TABLE `stubdatabasetransaction` DISABLE KEYS */;
/*!40000 ALTER TABLE `stubdatabasetransaction` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_trunk`.`table_engine`
--

DROP TABLE IF EXISTS `table_engine`;
CREATE TABLE `table_engine` (
  `ENGINE_ID` varchar(38) default NULL,
  `CAR_ID` varchar(38) default NULL,
  `ENGINE_NO` varchar(50) default NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_trunk`.`table_engine`
--

/*!40000 ALTER TABLE `table_engine` DISABLE KEYS */;
INSERT INTO `table_engine` (`ENGINE_ID`,`CAR_ID`,`ENGINE_NO`) VALUES 
 ('{10C2E8F2-B481-41F7-B672-46C5149692DB}','{D108CE2E-F804-4DE6-8962-5A32B8C8C470}','NO111');
/*!40000 ALTER TABLE `table_engine` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_trunk`.`testautoinc`
--

DROP TABLE IF EXISTS `testautoinc`;
CREATE TABLE `testautoinc` (
  `testautoincid` int(10) unsigned NOT NULL auto_increment,
  `testfield` varchar(45) NOT NULL default '',
  PRIMARY KEY  (`testautoincid`)
) ENGINE=InnoDB AUTO_INCREMENT=491 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_trunk`.`testautoinc`
--

/*!40000 ALTER TABLE `testautoinc` DISABLE KEYS */;
INSERT INTO `testautoinc` (`testautoincid`,`testfield`) VALUES 
 (1,'testing'),
 (467,'testing 123'),
 (468,'testing 123'),
 (469,'testing'),
 (470,'testing 123'),
 (471,'testing'),
 (472,'testing 123'),
 (473,'testing'),
 (474,'testing 123'),
 (475,'testing'),
 (476,'testing 123'),
 (477,'testing'),
 (478,'testing 123'),
 (479,'testing'),
 (480,'testing 123'),
 (481,'testing'),
 (482,'testing 123'),
 (483,'testing'),
 (484,'testing 123'),
 (485,'testing'),
 (486,'testing 123'),
 (487,'testing'),
 (488,'testing 123'),
 (489,'testing 123'),
 (490,'testing');
/*!40000 ALTER TABLE `testautoinc` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_trunk`.`testtableread`
--

DROP TABLE IF EXISTS `testtableread`;
CREATE TABLE `testtableread` (
  `TestTableReadData` varchar(50) default NULL,
  `TestTableReadID` varchar(38) default NULL,
  UNIQUE KEY `Index_1` (`TestTableReadData`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_trunk`.`testtableread`
--

/*!40000 ALTER TABLE `testtableread` DISABLE KEYS */;
INSERT INTO `testtableread` (`TestTableReadData`,`TestTableReadID`) VALUES 
 ('Test',NULL),
 ('Test2',NULL);
/*!40000 ALTER TABLE `testtableread` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_trunk`.`transactionlog`
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
) ENGINE=InnoDB AUTO_INCREMENT=1003 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_trunk`.`transactionlog`
--

/*!40000 ALTER TABLE `transactionlog` DISABLE KEYS */;
INSERT INTO `transactionlog` (`TransactionSequenceNo`,`DateTimeUpdated`,`WindowsUser`,`LogonUser`,`MachineName`,`BusinessObjectTypeName`,`CRUDAction`,`DirtyXML`) VALUES 
 (996,'2008-07-08 16:41:53','NT AUTHORITY\\SYSTEM','','CORE1','ContactPersonTransactionLogging','Created','<ContactPersonTransactionLogging ID=ContactPersonID={b78833eb-5a66-41c4-8867-d6bcf77d46da}><Properties><ContactPersonID><PreviousValue></PreviousValue><NewValue>{B78833EB-5A66-41C4-8867-D6BCF77D46DA}</NewValue></ContactPersonID><Surname><PreviousValue></PreviousValue><NewValue>6f434ebe-cf53-4fec-94f4-4c44556d5e57</NewValue></Surname><ContactPersonTransactionLogging>'),
 (997,'2008-07-08 16:41:53','NT AUTHORITY\\SYSTEM','','CORE1','ContactPersonTransactionLogging','Updated','<ContactPersonTransactionLogging ID=ContactPersonID={b78833eb-5a66-41c4-8867-d6bcf77d46da}><Properties><Surname><PreviousValue>6f434ebe-cf53-4fec-94f4-4c44556d5e57</PreviousValue><NewValue>828e83b5-1b25-487d-936a-5d35707c7d06</NewValue></Surname><ContactPersonTransactionLogging>'),
 (998,'2008-07-08 16:41:53','NT AUTHORITY\\SYSTEM','','CORE1','ContactPersonTransactionLogging','Updated','<ContactPersonTransactionLogging ID=ContactPersonID={b78833eb-5a66-41c4-8867-d6bcf77d46da}><Properties><Surname><PreviousValue>828e83b5-1b25-487d-936a-5d35707c7d06</PreviousValue><NewValue>e9118d6b-0e9e-40bc-acbb-21f52e1a7549</NewValue></Surname><ContactPersonTransactionLogging>');
INSERT INTO `transactionlog` (`TransactionSequenceNo`,`DateTimeUpdated`,`WindowsUser`,`LogonUser`,`MachineName`,`BusinessObjectTypeName`,`CRUDAction`,`DirtyXML`) VALUES 
 (999,'2008-07-08 16:41:53','NT AUTHORITY\\SYSTEM','','CORE1','ContactPersonTransactionLogging','Updated','<ContactPersonTransactionLogging ID=ContactPersonID={b78833eb-5a66-41c4-8867-d6bcf77d46da}><Properties><Surname><PreviousValue>e9118d6b-0e9e-40bc-acbb-21f52e1a7549</PreviousValue><NewValue>4abdc35e-80f0-436f-8309-000dec7436ca</NewValue></Surname><ContactPersonTransactionLogging>'),
 (1000,'2008-07-08 16:41:53','NT AUTHORITY\\SYSTEM','','CORE1','ContactPersonTransactionLogging','Updated','<ContactPersonTransactionLogging ID=ContactPersonID={b78833eb-5a66-41c4-8867-d6bcf77d46da}><Properties><Surname><PreviousValue>4abdc35e-80f0-436f-8309-000dec7436ca</PreviousValue><NewValue>8e3a92ea-a2c8-4d44-804f-5ce625e06334</NewValue></Surname><ContactPersonTransactionLogging>'),
 (1001,'2008-07-08 16:41:53','NT AUTHORITY\\SYSTEM','','CORE1','ContactPersonTransactionLogging','Deleted','<ContactPersonTransactionLogging ID=ContactPersonID={b78833eb-5a66-41c4-8867-d6bcf77d46da}><Properties><ContactPersonTransactionLogging>'),
 (1002,'2008-07-08 16:41:53','NT AUTHORITY\\SYSTEM','','CORE1','ContactPersonTransactionLogging','Created','<ContactPersonTransactionLogging ID=ContactPersonID={6eba7140-0d93-4eb1-8a2e-c59deee8a2aa}><Properties><ContactPersonID><PreviousValue></PreviousValue><NewValue>{6EBA7140-0D93-4EB1-8A2E-C59DEEE8A2AA}</NewValue></ContactPersonID><Surname><PreviousValue></PreviousValue><NewValue>My Surname 1</NewValue></Surname><ContactPersonTransactionLogging>');
/*!40000 ALTER TABLE `transactionlog` ENABLE KEYS */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
