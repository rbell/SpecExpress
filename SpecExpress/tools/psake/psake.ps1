# Helper script for those who want to run
# psake without importing the module.
import-module .\tools\psake\psake.psm1
invoke-psake @args
remove-module psake