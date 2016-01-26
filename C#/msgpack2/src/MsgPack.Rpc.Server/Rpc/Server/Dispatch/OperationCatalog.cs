#region -- License Terms --
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
using System.Diagnostics.Contracts;

namespace MsgPack.Rpc.Server.Dispatch
{
	/// <summary>
	///		Defines MessagePack-RPC method catalog as a dictionary like interface.
	/// </summary>
	public abstract class OperationCatalog : IEnumerable<OperationDescription>
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="OperationCatalog"/> class.
		/// </summary>
		protected OperationCatalog() { }

		/// <summary>
		///		Gets the <see cref="OperationDescription"/> for the specified method description.
		/// </summary>
		/// <param name="methodDescription">
		///		The method description. 
		///		The format is derived class specific.
		///	</param>
		/// <returns>
		///		The <see cref="OperationDescription"/> if the item for the specified method description  is found;
		///		<c>null</c>, otherwise.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="methodDescription"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="methodDescription"/> is empty or invalid.
		/// </exception>
		public OperationDescription Get( string methodDescription )
		{
			Validation.ValidateIsNotNullNorEmpty( methodDescription, "methodDescription" );

			Contract.EndContractBlock();

			return this.GetCore( methodDescription );
		}

		/// <summary>
		///		Gets the <see cref="OperationDescription"/> for the specified method description.
		/// </summary>
		/// <param name="methodDescription">
		///		The method description. 
		///		The format is derived class specific.
		///		This value is not null nor empty.
		///	</param>
		/// <returns>
		///		The <see cref="OperationDescription"/> if the item for the specified method description  is found;
		///		<c>null</c>, otherwise.
		/// </returns>
		/// <exception cref="ArgumentException">
		///		<paramref name="methodDescription"/> is invalid.
		/// </exception>
		protected abstract OperationDescription GetCore( string methodDescription );

		/// <summary>
		/// Determines whether [contains] [the specified method description].
		/// </summary>
		/// <param name="methodDescription">
		///		The method description. 
		///		The format is derived class specific.
		///	</param>
		/// <returns>
		///		<c>true</c> if the item for the specified method description  is found;
		///		<c>false</c>, otherwise.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="methodDescription"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="methodDescription"/> is empty.
		/// </exception>
		public bool Contains( string methodDescription )
		{
			return this.Get( methodDescription ) != null;
		}

		/// <summary>
		///		Adds the specified <see cref="OperationDescription"/> to this catalog.
		/// </summary>
		/// <param name="operation">
		///		The <see cref="OperationDescription"/> to be added.
		///	</param>
		/// <returns>
		///		<c>true</c> if the <paramref name="operation"/> is added successfully;
		///		<c>false</c>, if it is not added because the operation which has same id already exists in this catalog.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="operation"/> is <c>null</c>.
		/// </exception>
		public bool Add( OperationDescription operation )
		{
			if ( operation == null )
			{
				throw new ArgumentNullException( "operation" );
			}

			Contract.EndContractBlock();

			return this.AddCore( operation );
		}

		/// <summary>
		///		Adds the specified <see cref="OperationDescription"/> to this catalog.
		/// </summary>
		/// <param name="operation">
		///		The <see cref="OperationDescription"/> to be added.
		///		This value is not <c>null</c>.
		///	</param>
		/// <returns>
		///		<c>true</c> if the <paramref name="operation"/> is added successfully;
		///		<c>false</c>, if it is not added because the operation which has same id already exists.
		/// </returns>
		protected abstract bool AddCore( OperationDescription operation );

		/// <summary>
		///		Removes the specified <see cref="OperationDescription"/> from this catalog.
		/// </summary>
		/// <param name="operation">
		///		The <see cref="OperationDescription"/> to be removed.
		///	</param>
		/// <returns>
		///		<c>true</c> if the <paramref name="operation"/> is removed successfully;
		///		<c>false</c>, if it is not removed because the operation does not exist in this catalog.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="operation"/> is <c>null</c>.
		/// </exception>
		public bool Remove( OperationDescription operation )
		{
			if ( operation == null )
			{
				throw new ArgumentNullException( "operation" );
			}

			Contract.EndContractBlock();

			return this.RemoveCore( operation );
		}

		/// <summary>
		///		Removes the specified <see cref="OperationDescription"/> from this catalog.
		/// </summary>
		/// <param name="operation">
		///		The <see cref="OperationDescription"/> to be removed.
		///		This value is not <c>null</c>.
		///	</param>
		/// <returns>
		///		<c>true</c> if the <paramref name="operation"/> is removed successfully;
		///		<c>false</c>, if it is not removed because the operation does not exist in this catalog.
		/// </returns>
		protected abstract bool RemoveCore( OperationDescription operation );

		/// <summary>
		///		Clears all operations from this catalog.
		/// </summary>
		public abstract void Clear();

		/// <summary>
		///		Returns an enumerator to iterate all operations in this catalog.
		/// </summary>
		/// <returns>
		///		<see cref="T:System.Collections.Generic.IEnumerator`1"/> which can be used to interate all operations in this catalog.
		/// </returns>
		public abstract IEnumerator<OperationDescription> GetEnumerator();

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
