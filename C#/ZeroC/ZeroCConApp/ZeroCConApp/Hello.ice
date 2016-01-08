#pragma once

module Demo
{
	enum Color { red, green, blue };

	struct Structure
	{
		string name;
		Color value;
	};

	interface Hello
	{
		idempotent void sayHello(int delay);
		void shutdown();
		Structure get(string name);
	};

};
