
// ------------------------------------------------------------------------------
// This partial class was auto-generated for use with the Habanero Architecture.
// ------------------------------------------------------------------------------

using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Test.Structure
{
    public partial class Car
    {
        public new static ClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef = itsLoader.LoadClass(@"
			  <class name=""Car"" assembly=""Habanero.Test.Structure"" table=""table_Car"">
			    <property name=""CarID"" type=""Guid"" databaseField=""field_Car_ID"" />
			    <property name=""RegistrationNo"" databaseField=""field_Registration_No"" />
			    <property name=""Length"" type=""Double"" databaseField=""field_Length"" />
			    <property name=""IsConvertible"" type=""Boolean"" databaseField=""field_Is_Convertible"" />
			    <property name=""DriverID"" type=""Guid"" databaseField=""field_Driver_ID"" />
			    <primaryKey>
			      <prop name=""CarID"" />
			    </primaryKey>
			    <relationship name=""Driver"" type=""single"" relatedClass=""Person"" relatedAssembly=""Habanero.Test.Structure"">
			      <relatedProperty property=""DriverID"" relatedProperty=""PersonID"" />
			    </relationship>
			  </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public new static ClassDef LoadClassDef_WithClassTableInheritance()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef = itsLoader.LoadClass(@"
			  <class name=""Car"" assembly=""Habanero.Test.Structure"" table=""table_class_Car"">
			    <superClass class=""Vehicle"" assembly=""Habanero.Test.Structure"" />
			    <property name=""CarID"" type=""Guid"" databaseField=""field_Car_ID"" />
			    <property name=""RegistrationNo"" databaseField=""field_Registration_No"" />
			    <property name=""Length"" type=""Double"" databaseField=""field_Length"" />
			    <property name=""IsConvertible"" type=""Boolean"" databaseField=""field_Is_Convertible"" />
			    <property name=""DriverID"" type=""Guid"" databaseField=""field_Driver_ID"" />
			    <primaryKey>
			      <prop name=""CarID"" />
			    </primaryKey>
			    <relationship name=""Driver"" type=""single"" relatedClass=""Person"" relatedAssembly=""Habanero.Test.Structure"">
			      <relatedProperty property=""DriverID"" relatedProperty=""PersonID"" />
			    </relationship>
			  </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static Car CreateSavedCar()
        {
            Car car = CreateUnsavedCar();
            car.Save();
            return car;
        }

        public static Car CreateSavedCar(string registrationNo)
        {
            Car car = CreateUnsavedCar(registrationNo);
            car.Save();
            return car;
        }

        private static Car CreateUnsavedCar()
        {
            return CreateUnsavedCar(TestUtil.CreateRandomString());
        }

        private static Car CreateUnsavedCar(string registrationNo)
        {
            Car car = new Car();
            car.RegistrationNo = registrationNo;
            return car;
        }
    }
}