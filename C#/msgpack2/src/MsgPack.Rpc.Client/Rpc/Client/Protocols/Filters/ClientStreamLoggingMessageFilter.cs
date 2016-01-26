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
using MsgPack.Rpc.Diagnostics;
using MsgPack.Rpc.Protocols.Filters;

namespace MsgPack.Rpc.Client.Protocols.Filters
{
	/// <summary>
	///		<see cref="StreamLoggingMessageFilter{T}"/> for <see cref="ClientResponseContext" />.
	/// </summary>
	public sealed class ClientStreamLoggingMessageFilter : StreamLoggingMessageFilter<ClientResponseContext>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ClientStreamLoggingMessageFilter"/> class.
		/// </summary>
		/// <param name="logger">The <see cref="IMessagePackStreamLogger"/> which will be log sink.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="logger"/> is <c>null</c>.
		/// </exception>
		public ClientStreamLoggingMessageFilter( IMessagePackStreamLogger logger ) : base( logger ) { }
	}
}
