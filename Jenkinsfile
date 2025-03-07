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