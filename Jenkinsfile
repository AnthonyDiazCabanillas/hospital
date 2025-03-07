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
}