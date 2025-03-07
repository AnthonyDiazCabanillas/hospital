const path = require('path');

module.exports = {
  entry: './src/index.js', // Punto de entrada de la aplicación
  output: {
    filename: 'bundle.js', // Nombre del archivo de salida
    path: path.resolve(__dirname, 'dist'), // Carpeta de salida
  },
  mode: 'production', // Modo de construcción
};