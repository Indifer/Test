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
using MsgPack.Rpc.Protocols.Filters;

namespace MsgPack.Rpc.Client.Protocols.Filters
{
	/// <summary>
	///		<see cref="QuotaMessageFilter{T}"/> for <see cref="ClientResponseContext" />.
	/// </summary>
	public sealed class ClientQuotaMessageFilter : QuotaMessageFilter<ClientResponseContext>
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="ClientQuotaMessageFilter"/> class.
		/// </summary>
		/// <param name="quota">The quota. <c>0</c> means no quota (infinite).</param>
		/// <exception cref="ArgumentOutOfRangeException">
		///		The value of <paramref name="quota"/> is negative.
		/// </exception>
		public ClientQuotaMessageFilter( long quota ) : base( quota ) { }
	}
}