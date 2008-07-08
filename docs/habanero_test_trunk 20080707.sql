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
 ('{33297842-04A7-49E9-8C48-77B4D48D6B6A}',NULL,'5',NULL,NULL),
 ('{E2D04B59-EFF6-4DB2-BE10-7D3F720F4EC8}',NULL,'5',NULL,NULL),
 ('{A43C4482-BA8E-419B-B144-9F1E98F02D25}',NULL,'5',NULL,NULL),
 ('{93F4AECE-55DA-4CF7-A16C-3DC9B91B394F}',NULL,'5',NULL,NULL),
 ('{D72C7B07-8F93-459C-A782-6AD95BD6DB58}',NULL,'5',NULL,NULL),
 ('{0FA41B90-5BB8-4436-BFC0-C8DCE5812774}',NULL,'5',NULL,NULL),
 ('{22C6D554-95F4-4D8A-AFF1-9A6D182EB891}',NULL,'5',NULL,NULL),
 ('{43D75FF5-6332-41DB-BE74-A879FDE094C4}',NULL,'5',NULL,NULL),
 ('{5B2084D4-C927-4B9C-85B7-7B97967CC3D5}',NULL,'5',NULL,NULL),
 ('{D9F6BB93-D1DB-4FCF-95B2-361ABF1396A0}',NULL,'5',NULL,NULL),
 ('{EE967E20-39E6-43A4-ABB8-8558A970D96C}',NULL,'5',NULL,NULL),
 ('{FF540D55-43B5-4F7A-B02B-2BE9FDE83712}',NULL,'5',NULL,NULL),
 ('{81C0D196-9023-4BC6-8BF0-A1343A541F4B}',NULL,'5',NULL,NULL),
 ('{6D192334-71E7-4D16-BD08-168F23BDFA7B}',NULL,'5',NULL,NULL),
 ('{B5BF560F-20B4-4ADB-B68B-D3CD9597FD23}',NULL,'5',NULL,NULL),
 ('{0AA89305-7175-4AF9-8CF6-1FE67B01E1CB}',NULL,'5',NULL,NULL);
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
INSERT INTO `circle` (`CircleID`,`Radius`,`ShapeID`,`Colour`) VALUES 
 (NULL,10,'{8E4029BE-FAD9-49A2-AC3A-2A16A24C1F9D}',0),
 ('{E7163190-F95A-4CA2-8177-812315E8DFC9}',10,'{E7163190-F95A-4CA2-8177-812315E8DFC9}',0),
 ('{03909B65-BE26-410C-88D8-298BA0914FDA}',10,'{03909B65-BE26-410C-88D8-298BA0914FDA}',0),
 ('{A5C1A9E8-EEEA-4F63-BDFB-6C83989DE9CB}',10,'{A5C1A9E8-EEEA-4F63-BDFB-6C83989DE9CB}',0),
 ('{95124E62-55AF-4E11-BDC4-6E7B0DCEAA2B}',10,'{95124E62-55AF-4E11-BDC4-6E7B0DCEAA2B}',0),
 ('{5A11D24D-DA1E-4059-924D-0772594EC12A}',10,'{5A11D24D-DA1E-4059-924D-0772594EC12A}',0),
 ('{8474F445-EA51-4832-9E48-09B1CA8B003E}',10,NULL,0),
 ('{4D212F7E-E686-450B-A79D-AF20710E439D}',10,NULL,0),
 ('{EBCD50C2-8B57-4C37-A48E-BF7E869A9070}',10,'{EBCD50C2-8B57-4C37-A48E-BF7E869A9070}',0);
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
INSERT INTO `circle_concrete` (`CircleID`,`Radius`,`ShapeID`,`ShapeName`) VALUES 
 ('{D76871B8-E0F5-40A6-96BD-BB3D8DC9C4E6}',10,NULL,'c2c3c1ef-1d12-4247-af90-225fe44bd77f');
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
 ('{AC30F70D-FC28-45F3-A464-97CD35B10ADC}','32bf8a0911514b4a8dd79f8582969b86','7000b30bacde40668fac53f1330f48ba',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
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
 ('{038AE1A1-0F0C-468A-8410-7A90909CCEFF}','{86578275-B067-47D0-AC93-E1928370AAF8}','Surname','NewFirstName','1969-01-29 00:00:00','2008-06-26 10:09:17',NULL,NULL,1),
 ('{D8EB5628-6CF3-44AC-ACBD-2F0B4D000D29}','{6EDF26C6-C7F7-45D6-8204-3F2FEC6920E7}','Vincent','Brad','1980-01-22 00:00:00','2008-06-26 10:09:17',NULL,NULL,1),
 ('{D48795A0-B277-43DF-965D-8E8A99267817}','{A133C866-9E63-416B-9310-19EB42791059}','Vincent','Brad','1980-01-22 00:00:00','2008-06-26 10:09:17',NULL,NULL,1),
 ('{9971A0C7-0A72-4864-8440-4F5A4A0B8B73}','{4E7FDC1A-676A-4093-B2B2-C8DB3633E394}','Vincent','Brad','1980-01-22 00:00:00','2008-06-26 10:09:17',NULL,NULL,1),
 ('{05C66FA1-1FD2-4B1E-AC07-4CED5CC8E0EF}','{28E6DC2C-B99E-4E68-9B68-E3F198B6A706}','Vincent','Brad','1980-01-22 00:00:00','2008-06-26 10:09:17',NULL,NULL,1),
 ('de665631-1040-4c9b-a29c-9551ae371bd31','8f35575c-31a3-4752-9999-1f0bc1faa467','Vincent',NULL,NULL,'2008-06-26 10:09:17',NULL,NULL,1),
 ('de665631-1040-4c9b-a29c-9551ae371bd3','8f35575c-31a3-4752-9999-1f0bc1faa467','Vincent 2',NULL,NULL,'2008-06-26 10:09:17',NULL,NULL,1);
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
 ('{59054FA0-65E2-4156-A645-41837DCAE73A}','{2C0DC495-338F-4886-BDE7-1E83051A463A}',NULL);
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
 ('{0BCECB91-90F9-4A34-BF07-172C4785863B}','qwe','rty',NULL),
 ('{1092509E-DBA7-4C0B-BF60-65660EBDEB9E}','TestValue','TestValue2',NULL),
 ('{276658A3-0672-4858-B1DB-231A79A32EDF}',NULL,NULL,'{15E25589-1A16-4DB4-B909-40D7A83349BE}'),
 ('{2B23B008-F14B-4876-8270-75986AF85A6D}','TestValue','TestValue2',NULL),
 ('{33259EF8-0782-4EF4-AEB5-5FAB57414283}','qwe','rty',NULL),
 ('{347D4036-07E0-4F20-AB10-A59D354BFDAA}','TestValue','TestValue2',NULL),
 ('{380D6849-91A2-4F8E-A561-20ECF6B3D2AA}','qwe','rty',NULL),
 ('{434EC739-ECDC-4203-8509-C105A03DAEEA}','qwe','rty',NULL),
 ('{458CF3D9-A0A1-40DE-BC92-FE9515B4CAE8}','TestValue','TestValue2',NULL),
 ('{4A83B585-EF8E-4ED6-AD96-F38B36AD007F}','qwe','rty',NULL),
 ('{555580E1-5F97-45DC-A69B-68522E7B5EBD}','TestValue','TestValue2',NULL),
 ('{556316C6-F21F-419F-93D2-A6F28CAF3F96}','qwe','rty',NULL),
 ('{58BFAD39-AF56-47F1-AA9A-64584B43366D}',NULL,NULL,'{17113657-9590-4A59-B4E6-4F43E6EA6F63}'),
 ('{5AF8B03C-D41D-4120-9987-6E382F28D43A}','TestValue','TestValue2',NULL);
INSERT INTO `mybo` (`MyBoID`,`TestProp`,`TestProp2`,`ShapeID`) VALUES 
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
 ('{C2DC31F0-CC02-4F1B-B2FE-C03FB55E5B5A}',NULL,NULL,'{F872DA60-DB2A-4FED-9AC3-9897CE18BDD0}'),
 ('{CC8927F0-BA79-481D-BE40-797183ED8F38}',NULL,NULL,'{605BB5BA-F733-4BA9-85A5-D1E0165B20A9}'),
 ('{CF3E5C4C-CE6F-456D-A6B0-AABB971489C4}',NULL,NULL,'{78D739E1-8A16-4423-AD52-DC588DEDF2C9}');
INSERT INTO `mybo` (`MyBoID`,`TestProp`,`TestProp2`,`ShapeID`) VALUES 
 ('{CF4C814D-02CE-4F5E-B61B-840AF5CE3C63}','qwe','rty',NULL),
 ('{DBC79BCA-FCE3-46A4-8410-C2CCCFB977EA}','qwe','rty',NULL),
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
 (0,'GeneratedNumber',NULL,NULL,NULL,NULL,NULL),
 (0,'tmp','CHILLI\\Sherwin',0,'SHERWIN','CHILLI\\Sherwin','2008-07-07 17:14:35');
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
INSERT INTO `shape` (`ShapeID`,`ShapeName`,`ShapeType`,`Radius`,`Colour`) VALUES 
 ('{03909B65-BE26-410C-88D8-298BA0914FDA}','21d6eefe-2662-4164-b9ae-459ba06016e2',NULL,0,0),
 ('{286D587F-52D8-4BFC-B842-18C05231B0B6}','0233fce1-7dcd-4a5a-9199-5bc0520a8362','CircleNoPrimaryKey',10,0),
 ('{28D8B2C4-686D-4726-8BD3-074AD07D2E8A}','4b252305-5806-49f2-8922-722b8d678431','CircleNoPrimaryKey',10,0),
 ('{5A11D24D-DA1E-4059-924D-0772594EC12A}','2c648912-4087-42ff-ae1f-a30594342cf3',NULL,0,0),
 ('{8E4029BE-FAD9-49A2-AC3A-2A16A24C1F9D}','7e1ff56b-5edb-429a-9ef6-470cac98fbd9',NULL,0,0),
 ('{95124E62-55AF-4E11-BDC4-6E7B0DCEAA2B}','9b447ec1-8b58-4376-9d85-a528154fd073',NULL,0,0),
 ('{9F568F43-88BC-43F7-AF12-A97839EECDDD}','b67138cc-d97e-4c60-ac95-4a3301a80c79','CircleNoPrimaryKey',10,0),
 ('{A5C1A9E8-EEEA-4F63-BDFB-6C83989DE9CB}','c2d702c2-638d-486b-8f57-9641ed39c159',NULL,0,0),
 ('{B047CD4D-CC40-41C2-BF5E-10369AD348EC}','89734186-cb87-47f9-ad28-9dde14fd1a3e','CircleNoPrimaryKey',10,0),
 ('{B178E3CA-0462-4D91-910C-75B0A2B0BA37}','a57a45ff-dbf0-43be-967e-aea54ed54a8a','CircleNoPrimaryKey',10,0),
 ('{B2B4E3DE-390C-4A30-966A-41CC5CE79F49}',NULL,'CircleNoPrimaryKey',10,0);
INSERT INTO `shape` (`ShapeID`,`ShapeName`,`ShapeType`,`Radius`,`Colour`) VALUES 
 ('{B94E9DBF-8857-4064-867E-036F46D1AA62}','ad6bc9ff-f4f2-4375-9394-10a80b97fbd3','CircleNoPrimaryKey',10,0),
 ('{BEEA9B78-A390-4044-9EE8-A1C416F316A5}','4898d24f-0e59-4333-87d7-5b5a30854067','CircleNoPrimaryKey',10,0),
 ('{C0F04571-0B28-4467-8C63-59E1714DCCF0}','2aa541a7-13e0-44f6-96b1-a97e2ceb7c57','CircleNoPrimaryKey',10,0),
 ('{D667B1A5-F159-4625-8B56-CC6B3B7B6E07}','491b4702-ceeb-4632-8609-33921103c5e8','CircleNoPrimaryKey',10,0),
 ('{E2480606-251B-4468-BEFC-6F412C7C145E}','564fc0ef-d8bd-41d9-aeab-461c446614e4','CircleNoPrimaryKey',10,0),
 ('{E7163190-F95A-4CA2-8177-812315E8DFC9}','921db11d-ba9f-441a-ae4d-cc58f036da73',NULL,0,0),
 ('{EBCD50C2-8B57-4C37-A48E-BF7E869A9070}','3720b02c-4ab1-40c6-81b6-6229955ff5f6',NULL,0,0);
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
 ('{02BBA85A-35E2-4ABC-9153-97F400F676FA}','{33297842-04A7-49E9-8C48-77B4D48D6B6A}','20'),
 ('{42CD047E-7BA8-409B-9F99-27A0A36B9E2B}','{E2D04B59-EFF6-4DB2-BE10-7D3F720F4EC8}','20'),
 ('{77113D53-C795-4BC8-AB60-5DFD379E8ABA}','{A43C4482-BA8E-419B-B144-9F1E98F02D25}','20'),
 ('{E4858407-8310-4D2F-8716-FCF37D77292C}','{93F4AECE-55DA-4CF7-A16C-3DC9B91B394F}','20'),
 ('{9C1C49B1-08A1-44C3-9729-4BA9EDEE7172}','{D72C7B07-8F93-459C-A782-6AD95BD6DB58}','20'),
 ('{62B5DAEC-6503-4685-A183-58F0AE864E6A}','{0FA41B90-5BB8-4436-BFC0-C8DCE5812774}','20'),
 ('{5EE66E35-D477-4A78-897E-3F888F302A9A}','{22C6D554-95F4-4D8A-AFF1-9A6D182EB891}','20'),
 ('{36F1A6F0-F547-4173-97C4-19BB1DC3D54D}','{43D75FF5-6332-41DB-BE74-A879FDE094C4}','20'),
 ('{8CDAED22-C43D-4FE4-B5F9-366A772599DE}','{5B2084D4-C927-4B9C-85B7-7B97967CC3D5}','20'),
 ('{545746F3-7F32-4DF1-9599-C6A0AB96AFEC}','{D9F6BB93-D1DB-4FCF-95B2-361ABF1396A0}','20'),
 ('{B6106B9E-A728-42E7-912B-41D36AB4FA17}','{EE967E20-39E6-43A4-ABB8-8558A970D96C}','20'),
 ('{C07E2949-156C-42E2-9FEE-C26822C1DC4B}','{FF540D55-43B5-4F7A-B02B-2BE9FDE83712}','20');
INSERT INTO `table_engine` (`ENGINE_ID`,`CAR_ID`,`ENGINE_NO`) VALUES 
 ('{341A3438-2554-42D9-AF41-96D64DBE8E44}','{81C0D196-9023-4BC6-8BF0-A1343A541F4B}','20'),
 ('{8A15CCD5-050E-4DAD-A648-5698D61EB225}','{6D192334-71E7-4D16-BD08-168F23BDFA7B}','20'),
 ('{4223B67D-0429-40B7-813F-F4C5E5A4B9BD}','{B5BF560F-20B4-4ADB-B68B-D3CD9597FD23}','20'),
 ('{907EDC49-3DB9-453B-AD89-FE16CD988CD1}','{0AA89305-7175-4AF9-8CF6-1FE67B01E1CB}','20');
/*!40000 ALTER TABLE `table_engine` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_trunk`.`testautoinc`
--

DROP TABLE IF EXISTS `testautoinc`;
CREATE TABLE `testautoinc` (
  `testautoincid` int(10) unsigned NOT NULL auto_increment,
  `testfield` varchar(45) NOT NULL default '',
  PRIMARY KEY  (`testautoincid`)
) ENGINE=InnoDB AUTO_INCREMENT=489 DEFAULT CHARSET=latin1;

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
 (488,'testing 123');
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
) ENGINE=InnoDB AUTO_INCREMENT=991 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_trunk`.`transactionlog`
--

/*!40000 ALTER TABLE `transactionlog` DISABLE KEYS */;
INSERT INTO `transactionlog` (`TransactionSequenceNo`,`DateTimeUpdated`,`WindowsUser`,`LogonUser`,`MachineName`,`BusinessObjectTypeName`,`CRUDAction`,`DirtyXML`) VALUES 
 (984,'2008-07-07 17:14:44','CHILLI\\Sherwin','','SHERWIN','ContactPersonTransactionLogging','Created','<ContactPersonTransactionLogging ID=ContactPersonID={d15e506e-c91d-4c3f-95cb-d1510c55b854}><Properties><ContactPersonID><PreviousValue></PreviousValue><NewValue>{D15E506E-C91D-4C3F-95CB-D1510C55B854}</NewValue></ContactPersonID><Surname><PreviousValue></PreviousValue><NewValue>fd46b5f8-7bae-4ce0-9564-9b4c4bc032e6</NewValue></Surname><ContactPersonTransactionLogging>'),
 (985,'2008-07-07 17:14:44','CHILLI\\Sherwin','','SHERWIN','ContactPersonTransactionLogging','Updated','<ContactPersonTransactionLogging ID=ContactPersonID={d15e506e-c91d-4c3f-95cb-d1510c55b854}><Properties><Surname><PreviousValue>fd46b5f8-7bae-4ce0-9564-9b4c4bc032e6</PreviousValue><NewValue>5d1ac909-76a0-4010-9f36-61afbd147534</NewValue></Surname><ContactPersonTransactionLogging>'),
 (986,'2008-07-07 17:14:44','CHILLI\\Sherwin','','SHERWIN','ContactPersonTransactionLogging','Updated','<ContactPersonTransactionLogging ID=ContactPersonID={d15e506e-c91d-4c3f-95cb-d1510c55b854}><Properties><Surname><PreviousValue>5d1ac909-76a0-4010-9f36-61afbd147534</PreviousValue><NewValue>8ec4812c-2699-4902-bc99-351ff3a03686</NewValue></Surname><ContactPersonTransactionLogging>');
INSERT INTO `transactionlog` (`TransactionSequenceNo`,`DateTimeUpdated`,`WindowsUser`,`LogonUser`,`MachineName`,`BusinessObjectTypeName`,`CRUDAction`,`DirtyXML`) VALUES 
 (987,'2008-07-07 17:14:45','CHILLI\\Sherwin','','SHERWIN','ContactPersonTransactionLogging','Updated','<ContactPersonTransactionLogging ID=ContactPersonID={d15e506e-c91d-4c3f-95cb-d1510c55b854}><Properties><Surname><PreviousValue>8ec4812c-2699-4902-bc99-351ff3a03686</PreviousValue><NewValue>b8f650b3-9b7c-411a-a60e-9563b6d0d36d</NewValue></Surname><ContactPersonTransactionLogging>'),
 (988,'2008-07-07 17:14:45','CHILLI\\Sherwin','','SHERWIN','ContactPersonTransactionLogging','Updated','<ContactPersonTransactionLogging ID=ContactPersonID={d15e506e-c91d-4c3f-95cb-d1510c55b854}><Properties><Surname><PreviousValue>b8f650b3-9b7c-411a-a60e-9563b6d0d36d</PreviousValue><NewValue>6d4d1aa5-3e23-4252-828f-9cad4b4f551b</NewValue></Surname><ContactPersonTransactionLogging>'),
 (989,'2008-07-07 17:14:45','CHILLI\\Sherwin','','SHERWIN','ContactPersonTransactionLogging','Deleted','<ContactPersonTransactionLogging ID=ContactPersonID={d15e506e-c91d-4c3f-95cb-d1510c55b854}><Properties><ContactPersonTransactionLogging>'),
 (990,'2008-07-07 17:14:45','CHILLI\\Sherwin','','SHERWIN','ContactPersonTransactionLogging','Created','<ContactPersonTransactionLogging ID=ContactPersonID={71c6fef5-3816-4561-9113-6b3454d9596c}><Properties><ContactPersonID><PreviousValue></PreviousValue><NewValue>{71C6FEF5-3816-4561-9113-6B3454D9596C}</NewValue></ContactPersonID><Surname><PreviousValue></PreviousValue><NewValue>My Surname 1</NewValue></Surname><ContactPersonTransactionLogging>');
/*!40000 ALTER TABLE `transactionlog` ENABLE KEYS */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
