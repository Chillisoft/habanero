//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Habanero.BO")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AllowPartiallyTrustedCallers]
// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("E61DDFD7-FD7F-412b-8544-B9D75B58530A")]

[assembly: InternalsVisibleTo("Habanero.Test.BO, PublicKeyToken=7b53dd5883ca0b56, PublicKey=00240000048000009400000006020000002400005253413100040000010001009126953f2db9d54b13abba1eb18275db8309cf275f4e22ad469ba5f5064d5c1ef2a90fa94ab16316da85bb9fcb213502f297ad8802ca4bf8f0427ba791a0ece5645b63d707b208d9c0f5d55b7f7f348902aa17ee6991d954809a87bda38cdc29db6bda22e9181f0f9c35200512e591949ad0d40fb854a17d735cd4152a36079f")]
[assembly: InternalsVisibleTo("Habanero.Test, PublicKeyToken=c826c4967ee16d9f, PublicKey=0024000004800000940000000602000000240000525341310004000001000100998e334d635727dc279b113c47f37e3c88fd614456852c63e9efb0bfba24e6fa6c768b1b4c48a67922112e7ef237ead874f73d43404dc4452cf157a476877ef3c7a5df796e0e66b84bb185edcb87c660af2c2c8edd048f5eabad8c48bb20e31f2b5ceea942126583ba9f08c7228eda89525c72c7682898affae88f4cbdd74ea9")]
[assembly: InternalsVisibleTo("Habanero.Test.General, PublicKeyToken=bc82851abffae37d, PublicKey=0024000004800000940000000602000000240000525341310004000001000100bde6e58f56a49ba8db7014e4a75465368beeee1969e4ac3e86408034dd9e669ab00df6e736f4ac3baabc15570a3614d239422a87cd1fab7d8e12886660e554ad7f0326f5fa49d98307edea705031a3d23453d788cab6c71d0ea8f13ac8e56bfd121bcfd1f98e3615e031b417a0c3bae826c2f3bbc310655f4315b09ef98ae7b3")]
[assembly: InternalsVisibleTo("FireStarter.Bo, PublicKeyToken=2744fbe849ecfd9b, PublicKey=0024000004800000940000000602000000240000525341310004000001000100e98baf60a766c7d14daf60c656ef934dab4af444fee8c8460190f6a9aa5b23c6cb328fd893d9d14aefc018203a1cf920fb45786762b14855ad7ae2a83069ebc1f445a1f7d724fa4f0e32da60efd592f031c374538873ccfcd8c28d5cc16b8e6aaae15343b983eb11acd9f7205fa044e914c0c3f8fdef3498f20bc8a0df2087d5")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2, PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7")]
