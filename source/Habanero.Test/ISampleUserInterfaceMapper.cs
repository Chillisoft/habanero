using Habanero.BO.ClassDefinition;

namespace Habanero.Test
{
    public interface ISampleUserInterfaceMapper
    {
        UIDef GetUIDef();
        UIForm GetUIFormProperties();
        UIGrid GetUIGridProperties();
        UIForm SampleUserInterfaceMapper3Props();
        UIForm SampleUserInterfaceMapperPrivatePropOnly();
        UIForm SampleUserInterfaceMapperDescribedPropOnly(string toolTipText);
        UIForm SampleUserInterfaceMapper2Cols();
        UIForm SampleUserInterfaceMapper2Tabs();
        UIForm SampleUserInterfaceMapperColSpanning();
        UIForm SampleUserInterfaceMapperRowSpanning();
        UIForm SampleUserInterface_ReadWriteRule();
        UIForm SampleUserInterface_WriteNewRule();
    }
}