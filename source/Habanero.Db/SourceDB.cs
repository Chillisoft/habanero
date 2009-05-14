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

using Habanero.Base.Exceptions;

namespace Habanero.Base
{
    /// <summary>
    /// Represents a database source from which data is retrieved
    /// </summary>
    public class SourceDB : Source
    {
        private readonly Source _source;

        ///<summary>
        /// Constructor for SourceDB
        ///</summary>
        ///<param name="source"></param>
        public SourceDB(Source source)
            : base(source.Name, source.EntityName)
        {
            _source = source;
        }

        /// <summary>
        /// Gets and sets the name of the source
        /// </summary>
        public override string Name
        {
            get { return _source.Name; }
            set { _source.Name = value; }
        }

        /// <summary>
        /// Gets and sets the entity name of the source
        /// </summary>
        public override string EntityName
        {
            get { return _source.EntityName; }
            set { _source.EntityName = value; }
        }

        /// <summary>
        /// Gets the list of joins that make up the source
        /// </summary>
        public override JoinList Joins
        {
            get { return _source.Joins; }
        }

        /// <summary>
        /// Gets the list of joins that create one source through inheritance
        /// </summary>
        public override JoinList InheritanceJoins
        {
            get { return _source.InheritanceJoins; }
        }

        /// <summary>
        /// Creates the SQL that corresponds to this join
        /// </summary>
        public string CreateSQL()
        {
            return CreateSQL(new SqlFormatter("", "", "", ""));
        }

        private string GetJoinString(Source source, ISqlFormatter sqlFormatter)
        {
            string joinString = "";
            foreach (Join join in source.Joins)
            {
                joinString += " " + GetJoinString(join, sqlFormatter);
                if (join.ToSource.Joins.Count > 0)
                {
                    joinString += GetJoinString(join.ToSource, sqlFormatter);
                }
            }
            return joinString;
        }

        private string GetJoinString(Join join, ISqlFormatter sqlFormatter)
        {
            if (join.JoinFields.Count == 0)
            {
                string message = string.Format("SQL cannot be created for the source '{0}' because it has a join to '{1}' without join fields",
                                               Name, join.ToSource.Name);
                throw new HabaneroDeveloperException(message, "Please check how you are building your join clause structure.");
            }
            Join.JoinField joinField = join.JoinFields[0];
            string joinString = string.Format("{0} {1} ON {2}.{3} = {1}.{4}",
                                              join.GetJoinClause(),
                     sqlFormatter.DelimitTable(join.ToSource.EntityName), 
                     sqlFormatter.DelimitTable(join.FromSource.EntityName),
                     sqlFormatter.DelimitField(joinField.FromField.FieldName), 
                     sqlFormatter.DelimitField(joinField.ToField.FieldName));

            if (join.JoinFields.Count > 1)
            {
                for (int i = 1; i < join.JoinFields.Count; i++)
                {
                    joinField = join.JoinFields[i];
                    joinString += string.Format(" AND {0}.{2} = {1}.{3}", 
                        sqlFormatter.DelimitTable(join.FromSource.EntityName),  sqlFormatter.DelimitTable(join.ToSource.EntityName)    ,
                        sqlFormatter.DelimitField(joinField.FromField.FieldName), sqlFormatter.DelimitField(joinField.ToField.FieldName));
                }
                
            }
            
            return joinString;
        }

        /// <summary>
        /// Creates the Sql that corresponds to this join
        /// </summary>
        /// <param name="sqlFormatter">The formatter used to construct the appropriate Sql</param>
        public string CreateSQL(SqlFormatter sqlFormatter)
        {
            //if (Joins.Count == 0) return sqlFormatter.DelimitTable(EntityName);
            return GetTableJoinString(this, sqlFormatter) +  GetJoinString(this, sqlFormatter);
        }

        private string GetTableJoinString(Source source, ISqlFormatter sqlFormatter)
        {
            string joinString = sqlFormatter.DelimitTable(EntityName);
            joinString = GetInheritanceJoinString(sqlFormatter, source, joinString);
            return joinString;
        }

        private string GetInheritanceJoinString(ISqlFormatter sqlFormatter, Source source, string joinString)
        {
            foreach (Join join in source.InheritanceJoins)
            {
                joinString = "(" + joinString + " " + GetJoinString(join, sqlFormatter) + ")";
                if (join.ToSource.InheritanceJoins.Count > 0)
                {
                    joinString = GetInheritanceJoinString(sqlFormatter, join.ToSource, joinString);
                }
            }
            return joinString;
        }
    }
}