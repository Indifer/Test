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

namespace System.Diagnostics.Contracts
{
	/// <summary>
	///		Compatibility Mock.
	/// </summary>
	internal static class Contract
	{
		[Conditional( "DEBUG" )]
		public static void Assert( bool condition )
		{
			Debug.Assert( condition );
		}

		[Conditional( "DEBUG" )]
		public static void Assert( bool condition, string message )
		{
			Debug.Assert( condition, message );
		}

		[Conditional( "DEBUG" )]
		public static void Assume( bool condition )
		{
			Debug.Assert( condition );
		}

		[Conditional( "DEBUG" )]
		public static void Assume( bool condition, string message )
		{
			Debug.Assert( condition, message );
		}

		[Conditional( "DEBUG" )]
		public static void Requires( bool condition )
		{
			Debug.Assert( condition, "Precondition failed." );
		}

		[Conditional( "NEVER_COMPILED" )]
		public static void Ensures( bool condition ) { }

		[Conditional( "NEVER_COMPILED" )]
		public static void Invariant( bool condition ) { }

		public static T Result<T>()
		{
			return default( T );
		}

		public static T ValueAtReturn<T>( out T value )
		{
			value = default( T );
			return default( T );
		}

		public static T OldValue<T>( T value )
		{
			return default( T );
		}

		public static bool ForAll<T>( IEnumerable<T> collection, Predicate<T> predicate )
		{
			return true;
		}

		[Conditional( "NEVER_COMPILED" )]
		public static void EndContractBlock() { }
	}

	/// <summary>
	///		Compatibility Mock.
	/// </summary>
	[Conditional( "NEVER_COMPILED" )]
	[AttributeUsage(
		AttributeTargets.Class | AttributeTargets.Delegate
		| AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event
		| AttributeTargets.Parameter,
		Inherited = true
	)]
	internal sealed class PureAttribute : Attribute { }

	/// <summary>
	///		Compatibility Mock.
	/// </summary>
	[Conditional( "NEVER_COMPILED" )]
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Delegate )]
	internal sealed class ContractClassAttribute : Attribute
	{
		public ContractClassAttribute( Type typeContainingContracts ) { }
	}

	/// <summary>
	///		Compatibility Mock.
	/// </summary>
	[Conditional( "NEVER_COMPILED" )]
	[AttributeUsage( AttributeTargets.Class )]
	internal sealed class ContractClassForAttribute : Attribute
	{
		public ContractClassForAttribute( Type typeContractsAreFor ) { }
	}

	/// <summary>
	///		Compatibility Mock.
	/// </summary>
	[Conditional( "NEVER_COMPILED" )]
	[AttributeUsage( AttributeTargets.Method )]
	internal sealed class ContractInvariantMethodAttribute : Attribute
	{
		public ContractInvariantMethodAttribute() { }
	}
}
