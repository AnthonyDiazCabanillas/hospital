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
                sh 'npm install'
            }
        }

        /*stage('Ejecutar pruebas') {
            steps {
                sh 'npm test' // Asegúrate de que el proyecto tenga scripts de prueba en package.json
            }
        }*/

        stage('Construir proyecto') {
            steps {
                sh 'npm run build' // Asegúrate de que el proyecto tenga un script de construcción en package.json
            }
        }

        stage('Desplegar') {
            steps {
                script {
                    echo 'Deploying projects...'
                    def destinationDir = "D:\\DigitalizacionHC\\PruebaHospital"
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

/*pipeline {
    agent any

    tools {
        nodejs 'NodeJS' // Usa el nombre que configuraste en "Global Tool Configuration"
    }

    stages {
        stage('Clonar repositorio') {
            steps {
                git branch: 'main', url: 'https://github.com/AnthonyDiazCabanillas/hospital.git'
            }
        }

        stage('Instalar dependencias') {
            steps {
                sh 'npm install'
            }
        }

        stage('Ejecutar pruebas') {
            steps {
                sh 'npm test'
            }
        }

        stage('Construir proyecto') {
            steps {
                sh 'npm run build'
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
}*/