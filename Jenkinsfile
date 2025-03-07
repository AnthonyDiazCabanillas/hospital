pipeline {
    agent any

    environment {
        // Variables de entorno (puedes configurarlas en Jenkins)
        NODE_ENV = 'production'
        PORT = 3000 // Puerto para la aplicación (ajusta según tu proyecto)
    }

    stages {
        stage('Clonar repositorio') {
            steps {
                git branch: 'main', url: 'https://github.com/AnthonyDiazCabanillas/hospital.git'
            }
        }

        stage('Instalar dependencias') {
            steps {
                sh 'npm install' // Instala dependencias y devDependencies
            }
        }

        stage('Construir proyecto') {
            steps {
                sh 'npm run build' // Ejecuta el script de construcción
            }
        }
        
        stage('Deploy') {
            steps {
                script {
                    echo 'Deploying projects...'
                    def destinationDir = "D:\\DigitalizacionHC\\Prueba"
                    bat """
                        if not exist "${destinationDir}" (
                        mkdir "${destinationDir}"
                        )
                    """

                    // Ejemplo: Copiar archivos a un servidor remoto usando SCP
                    bat """
                        if exist "${PUBLISH_DIR}" (
                                    robocopy "${PUBLISH_DIR}" "${destinationDir}" /E                        
                        )
                    """
                    echo 'Projects deployed.'
                }
            }
        }



    }

    post {
        success {
            echo '¡Pipeline ejecutado con éxito!'
        }
        failure {
            echo 'Pipeline falló. Revisa los logs para más detalles.'
        }
    }
} 
/*
pipeline {
    agent any

    environment {
        // Especifica la versión de Node.js
        NODE_VERSION = '22.14'
    }

    stages {
        stage('Checkout') {
            steps {
                // Clona el repositorio
                git url: 'https://github.com/AnthonyDiazCabanillas/hospital.git', branch: 'main'
            }
        }

        stage('Setup Node.js') {
            steps {
                // Instala la versión especificada de Node.js
                nvmInstall nodeVersion: env.NODE_VERSION
                sh 'node --version'
                sh 'npm --version'
            }
        }

        stage('Install Dependencies') {
            steps {
                // Instala las dependencias del proyecto
                sh 'npm install'
            }
        }

        stage('Build') {
            steps {
                // Compila el proyecto
                sh 'npm run build'
            }
        }

        stage('Deploy') {
             script {
                    echo 'Deploying projects...'
                    def destinationDir = "D:\\DigitalizacionHC\\Prueba"
                    bat """
                        if not exist "${destinationDir}" (
                        mkdir "${destinationDir}"
                        )
                    """

                    // Ejemplo: Copiar archivos a un servidor remoto usando SCP
                    bat """
                        if exist "${PUBLISH_DIR}" (
                                    robocopy "${PUBLISH_DIR}" "${destinationDir}" /E                        
                        )
                    """
                    echo 'Projects deployed.'
                }
        }
    }

    post {
        success {
            // Notificación en caso de éxito
            echo '¡Despliegue exitoso!'
        }
        failure {
            // Notificación en caso de fallo
            echo 'Error en el despliegue.'
        }
    }
}*/