/*pipeline {
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
                    def sourceDir = "C:\\ProgramData\\Jenkins\\.jenkins\\workspace\\Hospital"
                    def destinationDir = "D:\\DigitalizacionHC\\PruebaHospital"
                    def logFile = "${env.WORKSPACE}\\robocopy.log" // Archivo de log completo
                    def failedLogFile = "${env.WORKSPACE}\\failed_copy.log" // Archivo de log solo con archivos no copiados

                    // Crear la carpeta de destino si no existe
                    bat """
                        if not exist "${destinationDir}" (
                            mkdir "${destinationDir}"
                        )
                    """

                    // Copiar todos los archivos desde la carpeta de origen a la de destino
                    bat """
                        robocopy "${sourceDir}" "${destinationDir}" /MIR /COPYALL /R:3 /W:5 /LOG:"${logFile}" /V /NP
                    """

                    // Filtrar el log para obtener solo los archivos no copiados
                    bat """
                        findstr /I /C:"ERROR" /C:"EXTRA" /C:"New File" /C:"*Mismatch*" "${logFile}" > "${failedLogFile}"
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
}*/

pipeline {
    agent any

    environment {
        // Configura las variables de entorno necesarias
        JAVA_HOME = '/usr/lib/jvm/java-11-openjdk-amd64' // Ajusta la ruta según tu entorno
        MAVEN_HOME = '/usr/share/maven' // Ajusta la ruta según tu entorno
        PATH = "${MAVEN_HOME}/bin:${JAVA_HOME}/bin:${PATH}"
    }

    stages {
        stage('Checkout') {
            steps {
                // Clona el repositorio
                git branch: 'main', url: 'https://github.com/AnthonyDiazCabanillas/hospital.git'
            }
        }

        stage('Compilar') {
            steps {
                // Compila el proyecto usando Maven
                sh 'mvn clean compile'
            }
        }

        stage('Test') {
            steps {
                // Ejecuta las pruebas unitarias
                sh 'mvn test'
            }
        }

        stage('Empaquetar') {
            steps {
                // Empaqueta el proyecto (genera el JAR/WAR)
                sh 'mvn package'
            }
        }
    }

    post {
        success {
            // Acciones a realizar si la construcción es exitosa
            echo '¡Construcción exitosa!'
        }
        failure {
            // Acciones a realizar si la construcción falla
            echo '¡Construcción fallida!'
        }
    }
}

