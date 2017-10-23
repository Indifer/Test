var names = ['Alice', 'Emily', 'Kate'];

React.render(
  <div>
  {
      names.map(function (name) {
          return <div>Hello, {name}!</div>
      })
  }
  </div>,
  document.getElementById('example')
);