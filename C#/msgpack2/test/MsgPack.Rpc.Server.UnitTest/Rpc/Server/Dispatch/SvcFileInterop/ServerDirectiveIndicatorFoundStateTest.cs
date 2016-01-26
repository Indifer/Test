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
using System.IO;
using NUnit.Framework;

namespace MsgPack.Rpc.Server.Dispatch.SvcFileInterop
{
	[TestFixture()]
	public class ServerDirectiveIndicatorFoundStateTest
	{
		[Test()]
		public void TestParse_AtImmediately_TransitToRuntimeDirectiveIndicatorFoundState()
		{
			SvcDirectiveParserState target = new ServerDirectiveIndicatorFoundState( GetPrevious() );
			var reader = new StringReader( "@" );
			target = target.Parse( reader );

			Assert.That( target, Is.TypeOf<RuntimeDirectiveIndicatorFoundState>() );
		}

		[Test()]
		public void TestParse_LeadingWhitespaceAndAtmmediately_StayAndFinallyTransitToRuntimeDirectiveIndicatorFoundState()
		{
			SvcDirectiveParserState target = new ServerDirectiveIndicatorFoundState( GetPrevious() );
			var reader = new StringReader( " @" );

			target = target.Parse( reader );

			Assert.That( target, Is.TypeOf<ServerDirectiveIndicatorFoundState>() );

			target = target.Parse( reader );
			Assert.That( target, Is.TypeOf<RuntimeDirectiveIndicatorFoundState>() );
		}

		[Test()]
		[ExpectedException( typeof( FormatException ) )]
		public void TestParse_NotAnAtChar()
		{
			SvcDirectiveParserState target = new ServerDirectiveIndicatorFoundState( GetPrevious() );
			var reader = new StringReader( "S" );

			target = target.Parse( reader );
		}

		private static StartTagFoundState GetPrevious()
		{
			return new StartTagFoundState( new InitialState() );
		}
	}
}
