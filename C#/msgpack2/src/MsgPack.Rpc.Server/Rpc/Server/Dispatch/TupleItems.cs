﻿#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
#endregion -- License Terms --

using System;
using System.Collections.Generic;
using System.Linq;

namespace MsgPack.Rpc.Server.Dispatch
{
	/// <summary>
	///		Defines helper method for items of tuple type.
	/// </summary>
	internal static class TupleItems
	{
		/// <summary>
		///		Creates type list for nested tuples.
		/// </summary>
		/// <param name="itemTypes">The type list of tuple items, in order.</param>
		/// <returns>
		///		The type list for nested tuples.
		///		The order is from outer to inner.
		/// </returns>
		public static List<Type> CreateTupleTypeList( IList<Type> itemTypes )
		{
			var itemTypesStack = new Stack<List<Type>>( itemTypes.Count / 7 + 1 );
			for ( int i = 0; i < itemTypes.Count / 7; i++ )
			{
				itemTypesStack.Push( itemTypes.Skip( i * 7 ).Take( 7 ).ToList() );
			}

			if ( itemTypes.Count % 7 != 0 )
			{
				itemTypesStack.Push( itemTypes.Skip( ( itemTypes.Count / 7 ) * 7 ).Take( itemTypes.Count % 7 ).ToList() );
			}

			var result = new List<Type>( itemTypesStack.Count );
			while ( 0 < itemTypesStack.Count )
			{
				var itemTypesStackEntry = itemTypesStack.Pop();
				if ( 0 < result.Count )
				{
					itemTypesStackEntry.Add( result.Last() );
				}

				var tupleType = Type.GetType( "System.Tuple`" + itemTypesStackEntry.Count, true ).MakeGenericType( itemTypesStackEntry.ToArray() );
				result.Add( tupleType );
			}

			result.Reverse();
			return result;
		}
	}

}
