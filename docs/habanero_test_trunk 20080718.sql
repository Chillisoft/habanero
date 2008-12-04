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
 ('{EE09D9C1-0A3A-4894-A901-1260CA490B3A}',NULL,'NP32459',NULL,NULL);
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
 ('{68BC990A-DBD9-4592-A642-4960980D3C9E}','{859A8A93-E9A3-4503-AE84-92FCEC58550B}','Surname','NewFirstName','1969-01-29 00:00:00','2008-07-10 20:08:08',NULL,NULL,1),
 ('{FACF6FB7-9587-41D6-B700-452A647A836A}','{FD02D876-1E9A-41D0-B079-A9E258A003E6}','Vincent','Brad','1980-01-22 00:00:00','2008-07-10 20:08:08',NULL,NULL,1),
 ('{90C9500B-9F22-4B64-9B61-90BF18ED6294}','{76508D72-A9DE-4D27-A05D-ABB370153857}','Vincent','Brad','1980-01-22 00:00:00','2008-07-10 20:08:08',NULL,NULL,1),
 ('{3B474F7D-42C6-4F56-9F58-6986E733E1D0}','{DEF09FC6-4B48-40AF-ACE0-202DDCBE6856}','Vincent','Brad','1980-01-22 00:00:00','2008-07-10 20:08:08',NULL,NULL,1),
 ('{179792FC-D082-4713-86B7-945E9CCDC04A}','{8E4940B6-CFB9-4AD2-BD7E-C5ABA071C5CF}','Vincent','Brad','1980-01-22 00:00:00','2008-07-10 20:08:08',NULL,NULL,1),
 ('1d7e0688-8201-4d1c-8dfe-94be862e69961','c5a9872d-e992-44a9-9b75-8f3febf3d22e','Vincent',NULL,NULL,'2008-07-10 20:08:08',NULL,NULL,1),
 ('1d7e0688-8201-4d1c-8dfe-94be862e6996','c5a9872d-e992-44a9-9b75-8f3febf3d22e','Vincent 2',NULL,NULL,'2008-07-10 20:08:08',NULL,NULL,1);
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
INSERT INTO `filledcircle_concrete` (`FilledCircleID`,`Colour`,`ShapeID`,`CircleID`,`Radius`,`ShapeName`) VALUES 
 ('{126A3DE6-8737-46BB-9CA5-79EB5764E566}',1,NULL,NULL,10,'f62f4e68-ea98-4522-9490-05f3129e2379'),
 ('{19363A72-4071-4D76-9344-500869C417D1}',1,NULL,NULL,10,'e552e05f-8ca1-41a9-a554-ef8da5736c7b'),
 ('{24D90FCF-68C0-400A-A6AE-B209839FC7BD}',1,NULL,NULL,10,'31042a71-0455-481f-9bc8-6f8ecc32900f'),
 ('{4ECB1A73-7687-45FB-873C-928F72B2BAAC}',1,NULL,NULL,10,'90b239af-4ea9-4b30-b57b-24a4ccb5e45b'),
 ('{7B605FA5-9007-4E1E-8B5A-27EB638FDF2A}',1,NULL,NULL,10,'113c4e9a-82d0-4410-bc22-1056f1bf18dc'),
 ('{A2706788-035D-4968-B41C-CE3F82D0E61B}',1,NULL,NULL,10,'b14af06d-a2ea-4719-8356-d6b9e8b6e721'),
 ('{C4065B9A-5E6B-4726-95DD-3441857C0DCD}',1,NULL,NULL,10,'1795025b-21b3-4a89-a8dc-7ab99854c0e6'),
 ('{CC5CD307-E02B-493D-935D-FF828C9BF480}',1,NULL,NULL,10,'7568e2c1-50e1-4e70-aa19-302f4c2dc347'),
 ('{F00B53FB-D797-4ECA-B1FE-03FAEE454FE2}',1,NULL,NULL,10,'3b284875-5c4c-4754-9be7-e3d5f25f2ef7');
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
 ('{00BECFBC-10F1-4FB9-90BF-EE467BC2837D}','{09BE5630-9DDC-4A77-A931-821064DD5E9F}',NULL);
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
 ('{44ED5445-9A1D-4BF2-9190-9A1F0776AB16}',NULL,NULL,'{D7374747-88F4-4008-A866-D91D912A4639}'),
 ('{458CF3D9-A0A1-40DE-BC92-FE9515B4CAE8}','TestValue','TestValue2',NULL),
 ('{47190EB3-0832-4264-B891-21073FAB6B98}','TestValue','TestValue2',NULL),
 ('{4A83B585-EF8E-4ED6-AD96-F38B36AD007F}','qwe','rty',NULL);
INSERT INTO `mybo` (`MyBoID`,`TestProp`,`TestProp2`,`ShapeID`) VALUES 
 ('{4BB69F91-3645-4017-9FA8-A0C222F2F02B}','qwe','rty',NULL),
 ('{555580E1-5F97-45DC-A69B-68522E7B5EBD}','TestValue','TestValue2',NULL),
 ('{556316C6-F21F-419F-93D2-A6F28CAF3F96}','qwe','rty',NULL),
 ('{58BFAD39-AF56-47F1-AA9A-64584B43366D}',NULL,NULL,'{17113657-9590-4A59-B4E6-4F43E6EA6F63}'),
 ('{5AF8B03C-D41D-4120-9987-6E382F28D43A}','TestValue','TestValue2',NULL),
 ('{5B471A24-B919-4043-B5C8-76ACF40243B7}','TestValue','TestValue2',NULL),
 ('{5F875157-7287-47C0-8508-AB17CEFD9A4A}',NULL,NULL,'{9DAE3A66-4A33-4C5A-8D0E-5EA5EB59B796}'),
 ('{6727E651-1B9A-4187-8F5E-92AEE84DB96D}',NULL,NULL,'{AF4F4CF2-2294-40B7-B8BE-1DA0333A8C22}'),
 ('{7B9672A7-446C-4619-A3C4-87015E3693FE}','TestValue','TestValue2',NULL),
 ('{84E43A45-7E05-4FC2-BC26-78AA09AE1269}',NULL,NULL,'{9EDF4E83-F505-495F-ADD9-7EEA1157C98D}'),
 ('{867C0713-7C35-4E5C-AD7D-C55C6970BA18}',NULL,NULL,'{28C77489-AEE0-48ED-8AB1-A666DEECFFA1}'),
 ('{89A82E77-2541-4305-942F-F4069563B2FF}','TestValue','TestValue2',NULL),
 ('{8E75AF97-FAFB-4D79-9502-B82411A59F65}','TestValue','TestValue2',NULL);
INSERT INTO `mybo` (`MyBoID`,`TestProp`,`TestProp2`,`ShapeID`) VALUES 
 ('{96EB0BF6-2DBC-4DEE-8249-80EE0671ADDC}','qwe','rty',NULL),
 ('{985F3556-1536-4761-B11E-ABBF2ABA8418}','qwe','rty',NULL),
 ('{A7405866-1FC5-436D-8988-86B789EE85FC}','qwe','rty',NULL),
 ('{AF71A742-9F74-4F64-8AD9-1F58351F8654}','TestValue','TestValue2',NULL),
 ('{B0AB8152-73D8-4749-A008-A4A497A8A4A1}',NULL,NULL,'{7BEE1972-7D7B-41CF-BBDF-09CFB2E25030}'),
 ('{BB76DA2A-3E4D-4294-945F-4E53AE4A8A78}','qwe','rty',NULL),
 ('{C2A19FEF-CC46-4844-9F4C-6EC782BCBDCB}',NULL,NULL,'{698BA923-B68F-4C22-AC71-3C7408A940EB}'),
 ('{C2DC31F0-CC02-4F1B-B2FE-C03FB55E5B5A}',NULL,NULL,'{F872DA60-DB2A-4FED-9AC3-9897CE18BDD0}'),
 ('{CC139616-FF91-452B-BC6A-48A4AEB1345B}','TestValue','TestValue2',NULL),
 ('{CC8927F0-BA79-481D-BE40-797183ED8F38}',NULL,NULL,'{605BB5BA-F733-4BA9-85A5-D1E0165B20A9}'),
 ('{CF3E5C4C-CE6F-456D-A6B0-AABB971489C4}',NULL,NULL,'{78D739E1-8A16-4423-AD52-DC588DEDF2C9}'),
 ('{CF4C814D-02CE-4F5E-B61B-840AF5CE3C63}','qwe','rty',NULL),
 ('{DBC79BCA-FCE3-46A4-8410-C2CCCFB977EA}','qwe','rty',NULL);
INSERT INTO `mybo` (`MyBoID`,`TestProp`,`TestProp2`,`ShapeID`) VALUES 
 ('{E40FDD73-1B1B-4F69-8276-0A9C9C1D350F}','TestValue','TestValue2',NULL),
 ('{E5FC5F53-669E-4A6D-A387-6AD87FBB76D5}',NULL,NULL,'{AF4338E7-45A9-44CA-82C6-261AD69FC3A7}'),
 ('{EAE21424-7FEF-4E27-9692-4F0DB514671C}','TestValue','TestValue2',NULL),
 ('{EB000E4A-5CCE-49ED-80FB-FA4FFB2A802A}',NULL,NULL,'{13AF46E7-51AC-46F3-B62E-B1391FE7F7BB}'),
 ('{F5EAAEEB-B056-4B04-8C40-92D7069077B9}','qwe','rty',NULL),
 ('{F6D88CCD-D29C-4EF4-86EB-6FAA228742D0}',NULL,NULL,'{CF6891C0-0C96-4E2B-B020-779639FFFE71}'),
 ('{F8E9FD56-06D4-4CF8-ADB4-6E7A8731C869}','qwe','rty',NULL),
 ('{FA2F5A02-3BC9-4B27-8E89-CB9F91CAE7BE}','TestValue','TestValue2',NULL);
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
 (0,'tmp','CHILLI\\brett',0,'BRETT','CHILLI\\brett','2008-07-10 20:07:44');
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
  `CircleType` varchar(45) default NULL,
  PRIMARY KEY  (`ShapeID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_trunk`.`shape`
--

/*!40000 ALTER TABLE `shape` DISABLE KEYS */;
INSERT INTO `shape` (`ShapeID`,`ShapeName`,`ShapeType`,`Radius`,`Colour`,`CircleType`) VALUES 
 ('{5106A8C5-41A2-48A4-9C83-C5D1287DC0AF}','1639fd53-a628-442b-8f08-77b02bbde95a','CircleNoPrimaryKey',10,0,NULL),
 ('{6B6125A0-0F1E-4E26-AF33-626602C88581}','ee1829f7-5845-4015-880f-ba1471d71c44','FilledCircleNoPrimaryKey',10,1,NULL),
 ('{EDCEFBF8-03D8-4F86-B3F3-C871C546DDC7}','d852140f-69c0-4258-bc43-b0cec04fcdbc','FilledCircleNoPrimaryKey',10,1,NULL);
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
 ('{F1A77DF4-8618-43F6-A44F-35556FC0FDA5}','{EE09D9C1-0A3A-4894-A901-1260CA490B3A}','NO111');
/*!40000 ALTER TABLE `table_engine` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_trunk`.`testautoinc`
--

DROP TABLE IF EXISTS `testautoinc`;
CREATE TABLE `testautoinc` (
  `testautoincid` int(10) unsigned NOT NULL auto_increment,
  `testfield` varchar(45) NOT NULL default '',
  PRIMARY KEY  (`testautoincid`)
) ENGINE=InnoDB AUTO_INCREMENT=499 DEFAULT CHARSET=latin1;

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
 (490,'testing'),
 (491,'testing 123'),
 (492,'testing'),
 (493,'testing 123'),
 (494,'testing'),
 (495,'testing 123'),
 (496,'testing'),
 (497,'testing 123'),
 (498,'testing');
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
  `BusinessObjectToString` varchar(255) default NULL,
  PRIMARY KEY  (`TransactionSequenceNo`)
) ENGINE=InnoDB AUTO_INCREMENT=1051 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_trunk`.`transactionlog`
--

/*!40000 ALTER TABLE `transactionlog` DISABLE KEYS */;
INSERT INTO `transactionlog` (`TransactionSequenceNo`,`DateTimeUpdated`,`WindowsUser`,`LogonUser`,`MachineName`,`BusinessObjectTypeName`,`CRUDAction`,`DirtyXML`,`BusinessObjectToString`) VALUES 
 (1044,'2008-07-10 20:07:49','CHILLI\\brett','','BRETT','ContactPersonTransactionLogging','Created','<ContactPersonTransactionLogging ID=ContactPersonID={638c2cdb-e915-4f27-a912-2b148980e38b}><Properties><ContactPersonID><PreviousValue></PreviousValue><NewValue>{638C2CDB-E915-4F27-A912-2B148980E38B}</NewValue></ContactPersonID><Surname><PreviousValue></PreviousValue><NewValue>b2420149-6cff-4920-89bf-b5dabe6857ae</NewValue></Surname><ContactPersonTransactionLogging>',NULL),
 (1045,'2008-07-10 20:07:49','CHILLI\\brett','','BRETT','ContactPersonTransactionLogging','Updated','<ContactPersonTransactionLogging ID=ContactPersonID={638c2cdb-e915-4f27-a912-2b148980e38b}><Properties><Surname><PreviousValue>b2420149-6cff-4920-89bf-b5dabe6857ae</PreviousValue><NewValue>b6d404d8-7547-4827-b469-35fc1af21c12</NewValue></Surname><ContactPersonTransactionLogging>',NULL),
 (1046,'2008-07-10 20:07:49','CHILLI\\brett','','BRETT','ContactPersonTransactionLogging','Updated','<ContactPersonTransactionLogging ID=ContactPersonID={638c2cdb-e915-4f27-a912-2b148980e38b}><Properties><Surname><PreviousValue>b6d404d8-7547-4827-b469-35fc1af21c12</PreviousValue><NewValue>b953e1b5-0bc3-41dd-ab27-342eb6fa20c8</NewValue></Surname><ContactPersonTransactionLogging>',NULL);
INSERT INTO `transactionlog` (`TransactionSequenceNo`,`DateTimeUpdated`,`WindowsUser`,`LogonUser`,`MachineName`,`BusinessObjectTypeName`,`CRUDAction`,`DirtyXML`,`BusinessObjectToString`) VALUES 
 (1047,'2008-07-10 20:07:49','CHILLI\\brett','','BRETT','ContactPersonTransactionLogging','Updated','<ContactPersonTransactionLogging ID=ContactPersonID={638c2cdb-e915-4f27-a912-2b148980e38b}><Properties><Surname><PreviousValue>b953e1b5-0bc3-41dd-ab27-342eb6fa20c8</PreviousValue><NewValue>d2a49e6f-93c3-4401-a68a-20e2b16b8269</NewValue></Surname><ContactPersonTransactionLogging>',NULL),
 (1048,'2008-07-10 20:07:49','CHILLI\\brett','','BRETT','ContactPersonTransactionLogging','Updated','<ContactPersonTransactionLogging ID=ContactPersonID={638c2cdb-e915-4f27-a912-2b148980e38b}><Properties><Surname><PreviousValue>d2a49e6f-93c3-4401-a68a-20e2b16b8269</PreviousValue><NewValue>525c781a-7c2e-4166-80ca-58577e241938</NewValue></Surname><ContactPersonTransactionLogging>',NULL),
 (1049,'2008-07-10 20:07:49','CHILLI\\brett','','BRETT','ContactPersonTransactionLogging','Deleted','<ContactPersonTransactionLogging ID=ContactPersonID={638c2cdb-e915-4f27-a912-2b148980e38b}><Properties><ContactPersonTransactionLogging>',NULL),
 (1050,'2008-07-10 20:07:49','CHILLI\\brett','','BRETT','ContactPersonTransactionLogging','Created','<ContactPersonTransactionLogging ID=ContactPersonID={15baf1e0-3711-41b3-a60e-6090f434104b}><Properties><ContactPersonID><PreviousValue></PreviousValue><NewValue>{15BAF1E0-3711-41B3-A60E-6090F434104B}</NewValue></ContactPersonID><Surname><PreviousValue></PreviousValue><NewValue>My Surname 1</NewValue></Surname><ContactPersonTransactionLogging>',NULL);
/*!40000 ALTER TABLE `transactionlog` ENABLE KEYS */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
