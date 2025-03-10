const path = require('path');

module.exports = {
  entry: './src/index.js', // Punto de entrada de tu aplicación
  output: {
    path: path.resolve(__dirname, 'dist'), // Carpeta de salida
    filename: 'bundle.js', // Nombre del archivo de salida
  },
  module: {
    rules: [
      {
        test: /\.(js|jsx)$/, // Aplica esta regla a archivos .js y .jsx
        exclude: /node_modules/, // Excluye la carpeta node_modules
        use: {
          loader: 'babel-loader', // Usa babel-loader para transpilar
        },
      },
    ],
  },
  resolve: {
    extensions: ['.js', '.jsx'], // Permite importar archivos .js y .jsx sin la extensión
  },
  mode: 'production', // Modo de producción
};