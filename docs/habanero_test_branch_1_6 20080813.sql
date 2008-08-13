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
-- Create schema habanero_test_branch_1_6
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ habanero_test_branch_1_6;
USE habanero_test_branch_1_6;

--
-- Table structure for table `habanero_test_branch_1_6`.`another_number_generator`
--

DROP TABLE IF EXISTS `another_number_generator`;
CREATE TABLE `another_number_generator` (
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
-- Dumping data for table `habanero_test_branch_1_6`.`another_number_generator`
--

/*!40000 ALTER TABLE `another_number_generator` DISABLE KEYS */;
INSERT INTO `another_number_generator` (`SequenceNumber`,`NumberType`,`UserLocked`,`Locked`,`MachineLocked`,`OperatingSystemUserLocked`,`DateTimeLocked`) VALUES 
 (1,'tmp',NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `another_number_generator` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_6`.`car`
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
-- Dumping data for table `habanero_test_branch_1_6`.`car`
--

/*!40000 ALTER TABLE `car` DISABLE KEYS */;
INSERT INTO `car` (`Car_ID`,`Owner_Id`,`Car_Reg_No`,`Driver_FK1`,`Driver_FK2`) VALUES 
 ('{50B71CF7-19E2-441D-B0D1-980D7AE2C156}',NULL,'NP32459',NULL,NULL);
/*!40000 ALTER TABLE `car` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_6`.`circle`
--

DROP TABLE IF EXISTS `circle`;
CREATE TABLE `circle` (
  `CircleID` char(38) default NULL,
  `Radius` int(10) unsigned NOT NULL default '0',
  `ShapeID` char(38) default NULL,
  `Colour` int(10) unsigned NOT NULL default '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_6`.`circle`
--

/*!40000 ALTER TABLE `circle` DISABLE KEYS */;
/*!40000 ALTER TABLE `circle` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_6`.`circle_concrete`
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
-- Dumping data for table `habanero_test_branch_1_6`.`circle_concrete`
--

/*!40000 ALTER TABLE `circle_concrete` DISABLE KEYS */;
/*!40000 ALTER TABLE `circle_concrete` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_6`.`contact_person`
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
-- Dumping data for table `habanero_test_branch_1_6`.`contact_person`
--

/*!40000 ALTER TABLE `contact_person` DISABLE KEYS */;
INSERT INTO `contact_person` (`ContactPersonID`,`Surname`,`FirstName`,`EmailAddress`,`PhoneNumber`,`CellNumber`,`DateOfBirth`,`DateLastUpdated`,`UserLastUpdated`,`MachineLastUpdated`,`VersionNumber`,`PK2_Prop1`,`PK2_Prop2`,`PK3_Prop`,`OrganisationID`,`UserLocked`,`DateTimeLocked`,`MachineLocked`,`OperatingSystemUserLocked`,`Locked`) VALUES 
 ('{2352B842-C6DE-40D9-B312-3DCB31242578}','Excude this one.',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
 ('{3E79B93A-D76B-4C75-BFA7-B695F80684BC}','bea1ad93dfde424b82cc00aa513040e5SSSSS',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
 ('{496C5068-A96D-4266-98BE-B9B65E3DA58C}','Include this one.',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
 ('{6AD0F830-A896-4048-84B0-4DB68821652B}','83897978f628487abc92d1df8f77ef8cSSSSS',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
 ('{E6AC5195-3846-4523-8358-6F9EED60A45A}','Excude this one.',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
INSERT INTO `contact_person` (`ContactPersonID`,`Surname`,`FirstName`,`EmailAddress`,`PhoneNumber`,`CellNumber`,`DateOfBirth`,`DateLastUpdated`,`UserLastUpdated`,`MachineLastUpdated`,`VersionNumber`,`PK2_Prop1`,`PK2_Prop2`,`PK3_Prop`,`OrganisationID`,`UserLocked`,`DateTimeLocked`,`MachineLocked`,`OperatingSystemUserLocked`,`Locked`) VALUES 
 ('{EC923AE0-5232-434B-AE8B-521BA95969B2}','3dc41ed58d6444638db9bf27afb3fdeb',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
 ('{F7835A16-D9AB-4BB4-938C-CE3198D1E504}','9fb3954d3e9841b3bb932313af63d14f',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
 ('{F81B4463-A533-4BA4-9B36-1BA0157DB78B}','Include this one.',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
 ('{FCEEF749-1506-4CA3-8301-4ACED6ACFC5F}','03021bb5309a45089367f5c19b035d2e',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `contact_person` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_6`.`contact_person_address`
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
-- Dumping data for table `habanero_test_branch_1_6`.`contact_person_address`
--

/*!40000 ALTER TABLE `contact_person_address` DISABLE KEYS */;
/*!40000 ALTER TABLE `contact_person_address` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_6`.`contactpersoncompositekey`
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
-- Dumping data for table `habanero_test_branch_1_6`.`contactpersoncompositekey`
--

/*!40000 ALTER TABLE `contactpersoncompositekey` DISABLE KEYS */;
INSERT INTO `contactpersoncompositekey` (`PK1_Prop1`,`PK1_Prop2`,`Surname`,`FirstName`,`DateOfBirth`,`DateLastUpdated`,`UserLastUpdated`,`MachineLastUpdated`,`VersionNumber`) VALUES 
 ('{53287D8A-9DB8-4B32-A8A5-E8B910660071}','{BBF66963-130A-48AF-ACAE-7B20D4F52B75}','Surname','NewFirstName','1969-01-29 00:00:00','2008-08-13 10:41:28',NULL,NULL,1),
 ('{F0DF2039-05A0-48B6-B405-7FBEF75C9C78}','{60CA00C3-3195-4475-B5E7-A7F9B02938A6}','Vincent','Brad','1980-01-22 00:00:00','2008-08-13 10:41:28',NULL,NULL,1),
 ('{459CF128-0076-4831-B334-B7BB9BB65AC7}','{740C6E9B-689C-4077-B98D-F1045A45926B}','Vincent','Brad','1980-01-22 00:00:00','2008-08-13 10:41:28',NULL,NULL,1),
 ('{2D816250-DF49-466B-B4F7-AB948C727DC5}','{D92F2E93-B71D-42DC-AC17-862AC8A180A4}','Vincent','Brad','1980-01-22 00:00:00','2008-08-13 10:41:28',NULL,NULL,1),
 ('{841645D5-BAE4-44BC-B9B6-C1B6C385C3C5}','{80C61BEC-6AA3-4F7D-8648-6F21BAD67741}','Vincent','Brad','1980-01-22 00:00:00','2008-08-13 10:41:28',NULL,NULL,1),
 ('39da32b5-e513-4176-b0be-d3c67e9cb2571','fcda44d2-1456-4cfb-8309-33c6116d45b2','Vincent',NULL,NULL,'2008-08-13 10:41:28',NULL,NULL,1),
 ('39da32b5-e513-4176-b0be-d3c67e9cb257','fcda44d2-1456-4cfb-8309-33c6116d45b2','Vincent 2',NULL,NULL,'2008-08-13 10:41:28',NULL,NULL,1);
/*!40000 ALTER TABLE `contactpersoncompositekey` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_6`.`filledcircle`
--

DROP TABLE IF EXISTS `filledcircle`;
CREATE TABLE `filledcircle` (
  `FilledCircleID` char(38) default NULL,
  `Colour` int(10) unsigned NOT NULL default '0',
  `ShapeID` char(38) default NULL,
  `CircleID` char(38) default NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_6`.`filledcircle`
--

/*!40000 ALTER TABLE `filledcircle` DISABLE KEYS */;
/*!40000 ALTER TABLE `filledcircle` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_6`.`filledcircle_concrete`
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
-- Dumping data for table `habanero_test_branch_1_6`.`filledcircle_concrete`
--

/*!40000 ALTER TABLE `filledcircle_concrete` DISABLE KEYS */;
INSERT INTO `filledcircle_concrete` (`FilledCircleID`,`Colour`,`ShapeID`,`CircleID`,`Radius`,`ShapeName`) VALUES 
 ('{126A3DE6-8737-46BB-9CA5-79EB5764E566}',1,NULL,NULL,10,'f62f4e68-ea98-4522-9490-05f3129e2379'),
 ('{19363A72-4071-4D76-9344-500869C417D1}',1,NULL,NULL,10,'e552e05f-8ca1-41a9-a554-ef8da5736c7b'),
 ('{24D90FCF-68C0-400A-A6AE-B209839FC7BD}',1,NULL,NULL,10,'31042a71-0455-481f-9bc8-6f8ecc32900f'),
 ('{4ECB1A73-7687-45FB-873C-928F72B2BAAC}',1,NULL,NULL,10,'90b239af-4ea9-4b30-b57b-24a4ccb5e45b'),
 ('{4F877EFF-2F69-4C2D-BE39-BB33D09A274A}',1,NULL,NULL,10,'cc786964-a0bc-4c6a-883b-2a6f59ad6f24'),
 ('{7B605FA5-9007-4E1E-8B5A-27EB638FDF2A}',1,NULL,NULL,10,'113c4e9a-82d0-4410-bc22-1056f1bf18dc'),
 ('{9DD6D9CF-71D3-4FBB-9F08-5DC100313857}',1,NULL,NULL,10,'e811ac52-2af0-4a1c-b0a1-5ad301121f8a'),
 ('{A2706788-035D-4968-B41C-CE3F82D0E61B}',1,NULL,NULL,10,'b14af06d-a2ea-4719-8356-d6b9e8b6e721'),
 ('{AB7A1DF6-AD93-4F40-9BC0-6CCC9BB3FC43}',1,NULL,NULL,10,'82b3c83d-e943-43d8-8d9c-099b9f034fed'),
 ('{C4065B9A-5E6B-4726-95DD-3441857C0DCD}',1,NULL,NULL,10,'1795025b-21b3-4a89-a8dc-7ab99854c0e6');
INSERT INTO `filledcircle_concrete` (`FilledCircleID`,`Colour`,`ShapeID`,`CircleID`,`Radius`,`ShapeName`) VALUES 
 ('{CC5CD307-E02B-493D-935D-FF828C9BF480}',1,NULL,NULL,10,'7568e2c1-50e1-4e70-aa19-302f4c2dc347'),
 ('{F00B53FB-D797-4ECA-B1FE-03FAEE454FE2}',1,NULL,NULL,10,'3b284875-5c4c-4754-9be7-e3d5f25f2ef7'),
 ('{FFAB7F46-01C3-48B9-B9CA-CAC93EC03606}',1,NULL,NULL,10,'16ec1667-a18a-4a73-839c-be8d3c496a41');
/*!40000 ALTER TABLE `filledcircle_concrete` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_6`.`mockbo`
--

DROP TABLE IF EXISTS `mockbo`;
CREATE TABLE `mockbo` (
  `MockBOID` varchar(38) default NULL,
  `MockBOProp1` varchar(38) default NULL,
  `MockBOProp2` varchar(50) default NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_6`.`mockbo`
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
INSERT INTO `mockbo` (`MockBOID`,`MockBOProp1`,`MockBOProp2`) VALUES 
 ('{723DDB66-6305-4A9F-A8A2-7FE5EE81AEFB}',NULL,NULL),
 ('{031AE503-B715-4A32-B656-C0995CC9EE39}',NULL,NULL),
 ('{05D22A91-F66D-4CEB-8EF5-8BC7807CE354}','{60B5EA0B-3CF7-4200-BDDA-AC291B94DB98}',NULL),
 ('{E44D6AA6-CEB1-414D-BCF7-67A615E7806D}',NULL,NULL),
 ('{1DA2ECD1-F472-4069-AAF4-BE3D6C7A211D}','{A60E9B06-DAD2-4034-8780-1C28EB9FB55B}',NULL),
 ('{CD6D8D5B-65A9-43A2-A485-975384407F12}',NULL,NULL),
 ('{DF1F7CCE-A131-4112-A93C-795CFB2FA12F}',NULL,NULL),
 ('{89C2C6B3-0B99-4CAF-BF0A-D2942C8341ED}','{FB907372-EB16-47E9-81BC-AFB08A218172}',NULL),
 ('{EAE22BFC-A0AE-40AB-915C-4E296F212B2D}',NULL,NULL),
 ('{D9095991-9035-4009-ACC9-2FAB3FCA0BE5}','{AE988F1D-620B-4DD6-B854-3F892FA99C31}',NULL),
 ('{B65DF782-3B79-444B-8518-BA00FB1B2D6C}',NULL,NULL),
 ('{6C49C6AD-A98C-4855-8870-A9EA8E4E8C90}',NULL,NULL),
 ('{0F955183-1B1B-40F5-B533-297411382807}','{D7A5C544-9555-43BF-AD25-7572FC59D428}',NULL),
 ('{B0F78B56-79CE-4BB0-867E-8EDAFF070C5E}',NULL,NULL),
 ('{C6FFE710-3198-426E-83ED-A84E473BCCA9}','{B4ABA688-873F-4A0C-A10B-21E98071BEA9}',NULL);
INSERT INTO `mockbo` (`MockBOID`,`MockBOProp1`,`MockBOProp2`) VALUES 
 ('{988B957F-3FB9-40B0-B705-FCA386E5B39C}',NULL,NULL),
 ('{D32DA350-79F5-4177-B99A-5B59FB02F17E}',NULL,NULL),
 ('{7AE9AFF5-B11B-4628-B3CD-8F532EC19A44}','{F696D6C4-A53F-4B67-B3EE-DA76FF69AC7C}',NULL),
 ('{B78B519E-2844-4147-BFDE-93819E0EC1DD}',NULL,NULL),
 ('{00BECFBC-10F1-4FB9-90BF-EE467BC2837D}','{09BE5630-9DDC-4A77-A931-821064DD5E9F}',NULL),
 ('{4656A93E-151B-44FB-AC2E-9FDF2F15CBF0}',NULL,NULL),
 ('{7EA4CE35-D85E-45A5-8B7F-D66CEC50F863}',NULL,NULL),
 ('{F7D2593D-913A-4C02-9735-6B493AD5849D}','{6535E29E-8F18-4A12-83A4-350D0395CE1A}',NULL),
 ('{FF20E347-9D64-45E2-8F72-053F17490FD8}',NULL,NULL),
 ('{1FC883E0-2567-4622-A49D-C8623864FB99}','{4D49520A-08D7-4F7C-B208-AA1B82EF5297}',NULL),
 ('{85D58B88-ECD6-408A-B48E-84177C564BF3}',NULL,NULL),
 ('{E80C7408-50D8-4889-9487-0592A37C00A5}',NULL,NULL),
 ('{B6A11A30-6AC7-41E3-B2C4-B7879564842B}','{98F49B98-0806-463C-A900-42831AEBABB0}',NULL),
 ('{1262ADAF-C05B-4629-942C-24E2CCF077B4}',NULL,NULL),
 ('{64969BD1-7BB9-41B7-B329-E45E84FB4A7D}','{E5BAE14D-24EC-4390-B4D8-6499AD0524E3}',NULL);
/*!40000 ALTER TABLE `mockbo` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_6`.`mybo`
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
-- Dumping data for table `habanero_test_branch_1_6`.`mybo`
--

/*!40000 ALTER TABLE `mybo` DISABLE KEYS */;
INSERT INTO `mybo` (`MyBoID`,`TestProp`,`TestProp2`,`ShapeID`) VALUES 
 ('{02B5C164-68CC-4040-B5CD-1F8A7DA55234}','TestValue','TestValue2',NULL),
 ('{0339ED51-11E4-440A-B6D4-38E2D516B968}',NULL,NULL,'{71D60CD7-5CC5-4ED3-AF3E-0FF865F3E247}'),
 ('{0BCECB91-90F9-4A34-BF07-172C4785863B}','qwe','rty',NULL),
 ('{1092509E-DBA7-4C0B-BF60-65660EBDEB9E}','TestValue','TestValue2',NULL),
 ('{15BB670C-30A4-4E8B-89DF-ED329C6C7E9C}',NULL,NULL,'{8B41D3C6-63E6-4F8F-B11A-77C686A6ED2C}'),
 ('{1A3EBB86-81A1-4801-832A-79CA912D1187}','qwe','rty',NULL),
 ('{276658A3-0672-4858-B1DB-231A79A32EDF}',NULL,NULL,'{15E25589-1A16-4DB4-B909-40D7A83349BE}'),
 ('{2B23B008-F14B-4876-8270-75986AF85A6D}','TestValue','TestValue2',NULL),
 ('{2B58BF6A-3184-4DA6-AF7C-0119A9D05CC0}','qwe','rty',NULL),
 ('{33259EF8-0782-4EF4-AEB5-5FAB57414283}','qwe','rty',NULL),
 ('{347D4036-07E0-4F20-AB10-A59D354BFDAA}','TestValue','TestValue2',NULL),
 ('{380D6849-91A2-4F8E-A561-20ECF6B3D2AA}','qwe','rty',NULL),
 ('{434EC739-ECDC-4203-8509-C105A03DAEEA}','qwe','rty',NULL),
 ('{44ED5445-9A1D-4BF2-9190-9A1F0776AB16}',NULL,NULL,'{D7374747-88F4-4008-A866-D91D912A4639}');
INSERT INTO `mybo` (`MyBoID`,`TestProp`,`TestProp2`,`ShapeID`) VALUES 
 ('{458CF3D9-A0A1-40DE-BC92-FE9515B4CAE8}','TestValue','TestValue2',NULL),
 ('{47190EB3-0832-4264-B891-21073FAB6B98}','TestValue','TestValue2',NULL),
 ('{4A83B585-EF8E-4ED6-AD96-F38B36AD007F}','qwe','rty',NULL),
 ('{4BB69F91-3645-4017-9FA8-A0C222F2F02B}','qwe','rty',NULL),
 ('{4DA24359-3DAD-42F2-9A3F-CA9107D8B666}',NULL,NULL,'{2D690093-1F96-467B-B6A5-0949C2412A59}'),
 ('{548316E3-7BB2-43B9-89F5-5AC6FD36B6DC}','TestValue','TestValue2',NULL),
 ('{555580E1-5F97-45DC-A69B-68522E7B5EBD}','TestValue','TestValue2',NULL),
 ('{556316C6-F21F-419F-93D2-A6F28CAF3F96}','qwe','rty',NULL),
 ('{58BFAD39-AF56-47F1-AA9A-64584B43366D}',NULL,NULL,'{17113657-9590-4A59-B4E6-4F43E6EA6F63}'),
 ('{5AF8B03C-D41D-4120-9987-6E382F28D43A}','TestValue','TestValue2',NULL),
 ('{5B471A24-B919-4043-B5C8-76ACF40243B7}','TestValue','TestValue2',NULL),
 ('{5F875157-7287-47C0-8508-AB17CEFD9A4A}',NULL,NULL,'{9DAE3A66-4A33-4C5A-8D0E-5EA5EB59B796}'),
 ('{639795C5-F1C4-41BF-B292-D95E4CF870A5}','TestValue','TestValue2',NULL),
 ('{6727E651-1B9A-4187-8F5E-92AEE84DB96D}',NULL,NULL,'{AF4F4CF2-2294-40B7-B8BE-1DA0333A8C22}');
INSERT INTO `mybo` (`MyBoID`,`TestProp`,`TestProp2`,`ShapeID`) VALUES 
 ('{7B9672A7-446C-4619-A3C4-87015E3693FE}','TestValue','TestValue2',NULL),
 ('{84E43A45-7E05-4FC2-BC26-78AA09AE1269}',NULL,NULL,'{9EDF4E83-F505-495F-ADD9-7EEA1157C98D}'),
 ('{867C0713-7C35-4E5C-AD7D-C55C6970BA18}',NULL,NULL,'{28C77489-AEE0-48ED-8AB1-A666DEECFFA1}'),
 ('{89A82E77-2541-4305-942F-F4069563B2FF}','TestValue','TestValue2',NULL),
 ('{8E75AF97-FAFB-4D79-9502-B82411A59F65}','TestValue','TestValue2',NULL),
 ('{916174D0-79A1-419A-BA1B-66F0E1094E01}','TestValue','TestValue2',NULL),
 ('{96EB0BF6-2DBC-4DEE-8249-80EE0671ADDC}','qwe','rty',NULL),
 ('{985F3556-1536-4761-B11E-ABBF2ABA8418}','qwe','rty',NULL),
 ('{A7405866-1FC5-436D-8988-86B789EE85FC}','qwe','rty',NULL),
 ('{AF71A742-9F74-4F64-8AD9-1F58351F8654}','TestValue','TestValue2',NULL),
 ('{B0AB8152-73D8-4749-A008-A4A497A8A4A1}',NULL,NULL,'{7BEE1972-7D7B-41CF-BBDF-09CFB2E25030}'),
 ('{BB76DA2A-3E4D-4294-945F-4E53AE4A8A78}','qwe','rty',NULL),
 ('{C2A19FEF-CC46-4844-9F4C-6EC782BCBDCB}',NULL,NULL,'{698BA923-B68F-4C22-AC71-3C7408A940EB}'),
 ('{C2DC31F0-CC02-4F1B-B2FE-C03FB55E5B5A}',NULL,NULL,'{F872DA60-DB2A-4FED-9AC3-9897CE18BDD0}');
INSERT INTO `mybo` (`MyBoID`,`TestProp`,`TestProp2`,`ShapeID`) VALUES 
 ('{CC139616-FF91-452B-BC6A-48A4AEB1345B}','TestValue','TestValue2',NULL),
 ('{CC8927F0-BA79-481D-BE40-797183ED8F38}',NULL,NULL,'{605BB5BA-F733-4BA9-85A5-D1E0165B20A9}'),
 ('{CCB83FC8-E80A-476A-AD14-9DF65B492462}','qwe','rty',NULL),
 ('{CF3E5C4C-CE6F-456D-A6B0-AABB971489C4}',NULL,NULL,'{78D739E1-8A16-4423-AD52-DC588DEDF2C9}'),
 ('{CF4C814D-02CE-4F5E-B61B-840AF5CE3C63}','qwe','rty',NULL),
 ('{D02A0840-495F-425C-A3B4-CCBC33156C01}','TestValue','TestValue2',NULL),
 ('{DBC79BCA-FCE3-46A4-8410-C2CCCFB977EA}','qwe','rty',NULL),
 ('{E40FDD73-1B1B-4F69-8276-0A9C9C1D350F}','TestValue','TestValue2',NULL),
 ('{E5FC5F53-669E-4A6D-A387-6AD87FBB76D5}',NULL,NULL,'{AF4338E7-45A9-44CA-82C6-261AD69FC3A7}'),
 ('{E97D7DB2-FFDF-419E-BFCB-EB85ACDCF97C}','TestValue','TestValue2',NULL),
 ('{EAE21424-7FEF-4E27-9692-4F0DB514671C}','TestValue','TestValue2',NULL),
 ('{EB000E4A-5CCE-49ED-80FB-FA4FFB2A802A}',NULL,NULL,'{13AF46E7-51AC-46F3-B62E-B1391FE7F7BB}'),
 ('{F5EAAEEB-B056-4B04-8C40-92D7069077B9}','qwe','rty',NULL);
INSERT INTO `mybo` (`MyBoID`,`TestProp`,`TestProp2`,`ShapeID`) VALUES 
 ('{F6D88CCD-D29C-4EF4-86EB-6FAA228742D0}',NULL,NULL,'{CF6891C0-0C96-4E2B-B020-779639FFFE71}'),
 ('{F8E9FD56-06D4-4CF8-ADB4-6E7A8731C869}','qwe','rty',NULL),
 ('{FA2F5A02-3BC9-4B27-8E89-CB9F91CAE7BE}','TestValue','TestValue2',NULL);
/*!40000 ALTER TABLE `mybo` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_6`.`numbergenerator`
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
-- Dumping data for table `habanero_test_branch_1_6`.`numbergenerator`
--

/*!40000 ALTER TABLE `numbergenerator` DISABLE KEYS */;
INSERT INTO `numbergenerator` (`SequenceNumber`,`NumberType`,`UserLocked`,`Locked`,`MachineLocked`,`OperatingSystemUserLocked`,`DateTimeLocked`) VALUES 
 (0,'GeneratedNumber',NULL,NULL,NULL,NULL,NULL),
 (0,'tmp','CHILLI\\brett',0,'LAPTOP','CHILLI\\brett','2008-08-13 10:41:10');
/*!40000 ALTER TABLE `numbergenerator` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_6`.`organisation`
--

DROP TABLE IF EXISTS `organisation`;
CREATE TABLE `organisation` (
  `OrganisationID` char(38) NOT NULL default '',
  PRIMARY KEY  (`OrganisationID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_6`.`organisation`
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
-- Table structure for table `habanero_test_branch_1_6`.`shape`
--

DROP TABLE IF EXISTS `shape`;
CREATE TABLE `shape` (
  `ShapeID` char(38) NOT NULL default '',
  `ShapeName` varchar(45) default NULL,
  `ShapeType` varchar(45) default NULL,
  `Radius` int(10) unsigned NOT NULL default '0',
  `Colour` int(10) unsigned NOT NULL default '0',
  `CircleType` varchar(45) default NULL,
  PRIMARY KEY  (`ShapeID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_6`.`shape`
--

/*!40000 ALTER TABLE `shape` DISABLE KEYS */;
/*!40000 ALTER TABLE `shape` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_6`.`stubdatabasetransaction`
--

DROP TABLE IF EXISTS `stubdatabasetransaction`;
CREATE TABLE `stubdatabasetransaction` (
  `id` varchar(255) default NULL,
  `name` varchar(255) default NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_6`.`stubdatabasetransaction`
--

/*!40000 ALTER TABLE `stubdatabasetransaction` DISABLE KEYS */;
/*!40000 ALTER TABLE `stubdatabasetransaction` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_6`.`table_engine`
--

DROP TABLE IF EXISTS `table_engine`;
CREATE TABLE `table_engine` (
  `ENGINE_ID` varchar(38) default NULL,
  `CAR_ID` varchar(38) default NULL,
  `ENGINE_NO` varchar(50) default NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_6`.`table_engine`
--

/*!40000 ALTER TABLE `table_engine` DISABLE KEYS */;
INSERT INTO `table_engine` (`ENGINE_ID`,`CAR_ID`,`ENGINE_NO`) VALUES 
 ('{8B1B8BC9-AA5E-480B-AF26-B09AB8F49F0D}','{50B71CF7-19E2-441D-B0D1-980D7AE2C156}','NO111');
/*!40000 ALTER TABLE `table_engine` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_6`.`testautoinc`
--

DROP TABLE IF EXISTS `testautoinc`;
CREATE TABLE `testautoinc` (
  `testautoincid` int(10) unsigned NOT NULL auto_increment,
  `testfield` varchar(45) NOT NULL default '',
  PRIMARY KEY  (`testautoincid`)
) ENGINE=InnoDB AUTO_INCREMENT=503 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_6`.`testautoinc`
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
 (490,'testing'),
 (491,'testing 123'),
 (492,'testing'),
 (493,'testing 123'),
 (494,'testing'),
 (495,'testing 123'),
 (496,'testing'),
 (497,'testing 123'),
 (498,'testing'),
 (499,'testing 123'),
 (500,'testing'),
 (501,'testing 123'),
 (502,'testing');
/*!40000 ALTER TABLE `testautoinc` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_6`.`testtableread`
--

DROP TABLE IF EXISTS `testtableread`;
CREATE TABLE `testtableread` (
  `TestTableReadData` varchar(50) default NULL,
  `TestTableReadID` varchar(38) default NULL,
  UNIQUE KEY `Index_1` (`TestTableReadData`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_6`.`testtableread`
--

/*!40000 ALTER TABLE `testtableread` DISABLE KEYS */;
INSERT INTO `testtableread` (`TestTableReadData`,`TestTableReadID`) VALUES 
 ('Test',NULL),
 ('Test2',NULL);
/*!40000 ALTER TABLE `testtableread` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_1_6`.`transactionlog`
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
  `BusinessObjectToString` varchar(255) default NULL,
  PRIMARY KEY  (`TransactionSequenceNo`)
) ENGINE=InnoDB AUTO_INCREMENT=1075 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_1_6`.`transactionlog`
--

/*!40000 ALTER TABLE `transactionlog` DISABLE KEYS */;
INSERT INTO `transactionlog` (`TransactionSequenceNo`,`DateTimeUpdated`,`WindowsUser`,`LogonUser`,`MachineName`,`BusinessObjectTypeName`,`CRUDAction`,`DirtyXML`,`BusinessObjectToString`) VALUES 
 (1068,'2008-08-13 10:41:13','CHILLI\\brett','MyUserName','LAPTOP','ContactPersonTransactionLogging','Created','<ContactPersonTransactionLogging ID=\'ContactPersonID={cbaf794d-27f5-44a6-88d7-9cad53850147}\'><Properties><ContactPersonID><PreviousValue></PreviousValue><NewValue>{CBAF794D-27F5-44A6-88D7-9CAD53850147}</NewValue></ContactPersonID><Surname><PreviousValue></PreviousValue><NewValue>66c2b2ab-ae79-40c4-a909-3c3b0942bbec</NewValue></Surname></Properties></ContactPersonTransactionLogging>','66c2b2ab-ae79-40c4-a909-3c3b0942bbec'),
 (1069,'2008-08-13 10:41:13','CHILLI\\brett','MyUserName','LAPTOP','ContactPersonTransactionLogging','Updated','<ContactPersonTransactionLogging ID=\'ContactPersonID={cbaf794d-27f5-44a6-88d7-9cad53850147}\'><Properties><Surname><PreviousValue>66c2b2ab-ae79-40c4-a909-3c3b0942bbec</PreviousValue><NewValue>89d51a27-fc4d-4acb-ad60-1e0c90cff567</NewValue></Surname></Properties></ContactPersonTransactionLogging>','89d51a27-fc4d-4acb-ad60-1e0c90cff567'),
 (1070,'2008-08-13 10:41:13','CHILLI\\brett','MyUserName','LAPTOP','ContactPersonTransactionLogging','Updated','<ContactPersonTransactionLogging ID=\'ContactPersonID={cbaf794d-27f5-44a6-88d7-9cad53850147}\'><Properties><Surname><PreviousValue>89d51a27-fc4d-4acb-ad60-1e0c90cff567</PreviousValue><NewValue>8a634145-9bb3-4763-96d8-c853b4444f23</NewValue></Surname></Properties></ContactPersonTransactionLogging>','8a634145-9bb3-4763-96d8-c853b4444f23');
INSERT INTO `transactionlog` (`TransactionSequenceNo`,`DateTimeUpdated`,`WindowsUser`,`LogonUser`,`MachineName`,`BusinessObjectTypeName`,`CRUDAction`,`DirtyXML`,`BusinessObjectToString`) VALUES 
 (1071,'2008-08-13 10:41:13','CHILLI\\brett','MyUserName','LAPTOP','ContactPersonTransactionLogging','Updated','<ContactPersonTransactionLogging ID=\'ContactPersonID={cbaf794d-27f5-44a6-88d7-9cad53850147}\'><Properties><Surname><PreviousValue>8a634145-9bb3-4763-96d8-c853b4444f23</PreviousValue><NewValue>31add17e-4d19-4fbe-924a-05f1ef15e6ca</NewValue></Surname></Properties></ContactPersonTransactionLogging>','31add17e-4d19-4fbe-924a-05f1ef15e6ca'),
 (1072,'2008-08-13 10:41:13','CHILLI\\brett','MyUserName','LAPTOP','ContactPersonTransactionLogging','Updated','<ContactPersonTransactionLogging ID=\'ContactPersonID={cbaf794d-27f5-44a6-88d7-9cad53850147}\'><Properties><Surname><PreviousValue>31add17e-4d19-4fbe-924a-05f1ef15e6ca</PreviousValue><NewValue>30c91728-daf9-4eb7-b28c-3c6ab2930b86</NewValue></Surname></Properties></ContactPersonTransactionLogging>','30c91728-daf9-4eb7-b28c-3c6ab2930b86'),
 (1073,'2008-08-13 10:41:13','CHILLI\\brett','MyUserName','LAPTOP','ContactPersonTransactionLogging','Deleted','<ContactPersonTransactionLogging ID=\'ContactPersonID={cbaf794d-27f5-44a6-88d7-9cad53850147}\'><Properties></Properties></ContactPersonTransactionLogging>','30c91728-daf9-4eb7-b28c-3c6ab2930b86');
INSERT INTO `transactionlog` (`TransactionSequenceNo`,`DateTimeUpdated`,`WindowsUser`,`LogonUser`,`MachineName`,`BusinessObjectTypeName`,`CRUDAction`,`DirtyXML`,`BusinessObjectToString`) VALUES 
 (1074,'2008-08-13 10:41:13','CHILLI\\brett','MyUserName','LAPTOP','ContactPersonTransactionLogging','Created','<ContactPersonTransactionLogging ID=\'ContactPersonID={6e0552a2-de0d-4115-b487-b33fc6d10069}\'><Properties><ContactPersonID><PreviousValue></PreviousValue><NewValue>{6E0552A2-DE0D-4115-B487-B33FC6D10069}</NewValue></ContactPersonID><Surname><PreviousValue></PreviousValue><NewValue>My Surname 1</NewValue></Surname></Properties></ContactPersonTransactionLogging>','My Surname 1');
/*!40000 ALTER TABLE `transactionlog` ENABLE KEYS */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
